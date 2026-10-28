export interface Patient {
  idPatient: number;
  namePatient: string;
}

export interface Exam {
  idExam: number;
  codeExam: string;
  nameExam: string;
}

export interface Order {
  idOrder: number;
  namePatient: string;
  dateAtention: string;
  exams: Exam[];
}

export interface CreateOrderRequest {
  idPatient: number;
  dateAtention: string;
  idExams: number[];
}

export interface CreateOrderResponse {
  idOrder: number;
}