import { Injectable, inject } from '@angular/core';
import { Observable } from 'rxjs';
import { CreateOrderRequest, CreateOrderResponse, Exam, Order, Patient } from '../models/models';
import { ApiService } from './api.service';

@Injectable({ providedIn: 'root' })
export class OrderService {
  private readonly api = inject(ApiService);

  getOrders(): Observable<Order[]> {
    return this.api.get<Order[]>('orders');
  }

  getPatients(): Observable<Patient[]> {
    return this.api.get<Patient[]>('patients');
  }

  getExams(): Observable<Exam[]> {
    return this.api.get<Exam[]>('exams');
  }

  createOrder(order: CreateOrderRequest): Observable<CreateOrderResponse> {
    return this.api.post<CreateOrderResponse>('orders', order);
  }
}