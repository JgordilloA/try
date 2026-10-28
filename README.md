# Laboratorio App

API ASP.NET Core + Angular + SQL Server Express para registrar y listar órdenes de laboratorio.

```
laboratorio-app/
├── backend/    API (.NET 9) -> backend/Laboratorio.Api
├── frontend/   Angular 19
├── database/   Scripts SQL
```

## Requisitos
.NET SDK 9, Node.js + Angular CLI, SQL Server Express (`localhost\SQLEXPRESS`, Windows Authentication).

## Base de datos
Crea `LaboratorioDb` y ejecuta `database/schema.sql` desde SSMS (tablas `Patient`, `Orders`, `Exam`, `OrdersExam` y datos de ejemplo).
La cadena de conexión está en `backend/Laboratorio.Api/appsettings.json`.

## Backend (http://localhost:5000)
```bash
cd backend/Laboratorio.Api
dotnet run
```

| Método | Ruta | Descripción |
|---|---|---|
| GET | `/api/health` | Comprueba que la API responde |
| GET | `/api/orders` | Lista las órdenes con sus exámenes |
| POST | `/api/orders` | Crea una orden `{ "idPatient": 1, "dateAtention": "2026-10-20", "idExams": [1, 2] }` |
| GET | `/api/patients` | Lista de pacientes |
| GET | `/api/exams` | Lista de exámenes |
| POST | `/api/login` | `{ "user": "demo", "password": "1234" }` devuelve un token simulado |

Estructura: `Controllers` (HTTP) → `Services` (lógica y SQL con Dapper) · `Middleware` (registro de solicitudes en consola y manejo de errores) · `Models`/`Interfaces`/`Exceptions`.
Los errores devuelven `{ "message": "..." }` con 400 (datos inválidos), 404 (paciente/examen inexistente), 401 (login) o 500.
La creación de una orden se ejecuta en una transacción.

## Frontend (http://localhost:4200)
```bash
cd frontend
npm install
npm start
```
La URL de la API está en `frontend/src/environments/environment.ts`. CORS se configura en `appsettings.json` (`Cors:AllowedOrigins`).