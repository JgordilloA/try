import { DatePipe } from '@angular/common';
import { HttpErrorResponse } from '@angular/common/http';
import { Component, OnInit, inject } from '@angular/core';
import { AbstractControl, FormBuilder, ReactiveFormsModule, ValidationErrors, Validators } from '@angular/forms';
import { Exam, Order, Patient } from '../models/models';
import { OrderService } from '../services/order.service';

// Fecha local de hoy en formato yyyy-MM-dd
const today = () => new Date().toLocaleDateString('en-CA');

function notFutureDate(control: AbstractControl): ValidationErrors | null {
  return control.value > today() ? { futureDate: true } : null;
}

@Component({
  selector: 'app-orders',
  standalone: true,
  imports: [ReactiveFormsModule, DatePipe],
  templateUrl: './orders.component.html',
  styleUrl: './orders.component.css'
})
export class OrdersComponent implements OnInit {
  private readonly orderService = inject(OrderService);

  readonly today = today();
  patients: Patient[] = [];
  exams: Exam[] = [];
  orders: Order[] = [];
  error: string | null = null;

  form = inject(FormBuilder).nonNullable.group({
    idPatient: [null as number | null, Validators.required],
    dateAtention: [today(), [Validators.required, notFutureDate]],
    idExams: [[] as number[], Validators.required] // required también rechaza listas vacías
  });

  ngOnInit(): void {
    this.orderService.getPatients().subscribe({ next: r => (this.patients = r), error: e => this.showError(e) });
    this.orderService.getExams().subscribe({ next: r => (this.exams = r), error: e => this.showError(e) });
    this.loadOrders();
  }

  loadOrders(): void {
    this.orderService.getOrders().subscribe({ next: r => (this.orders = r), error: e => this.showError(e) });
  }

  toggleExam(idExam: number, checked: boolean): void {
    const control = this.form.controls.idExams;
    control.setValue(checked ? [...control.value, idExam] : control.value.filter(id => id !== idExam));
    control.markAsTouched();
  }

  submit(): void {
    this.error = null;
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }

    const { idPatient, dateAtention, idExams } = this.form.getRawValue();
    this.orderService.createOrder({ idPatient: idPatient!, dateAtention, idExams }).subscribe({
      next: () => {
        this.form.reset({ idPatient: null, dateAtention: today(), idExams: [] });
        this.loadOrders();
      },
      error: e => this.showError(e)
    });
  }

  private showError(err: HttpErrorResponse): void {
    this.error =
      err.status === 0
        ? 'No se pudo conectar con el backend. Verifica que la API esté en ejecución.'
        : err.error?.message ?? 'Ocurrió un error al comunicarse con el servidor.';
  }
}