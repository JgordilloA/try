-- Ejecutar en SSMS sobre LaboratorioDb
-- create database LaboratorioDb;
-- use LaboratorioDb;

create table Patient(
    IdPatient int identity not null,
    NamePatient nvarchar(100) not null,
    constraint PK_IdPatient primary key (IdPatient)
);

create table Orders(
    IdOrder int identity not null,
    IdPatient int not null,
    DateOrder date not null constraint DF_DateOrder default (GETDATE()),
    constraint PK_IdOrder primary key (IdOrder),
    constraint FK_IdPatient foreign key (IdPatient) references Patient(IdPatient)
);

create table Exam(
    IdExam int identity not null,
    CodeExam nvarchar(50) not null,
    NameExam nvarchar(50) not null,
    constraint PK_IdExam primary key (IdExam)
);

create table OrdersExam(
    IdOrder int not null,
    IdExam int not null,
    constraint PK_IdOrder_IdExam primary key (IdOrder, IdExam),
    constraint FK_IdOrder_OrdersExam foreign key (IdOrder) references Orders(IdOrder),
    constraint FK_IdExam_OrdersExam foreign key (IdExam) references Exam(IdExam)
);

-- Datos de ejemplo
insert into Patient (NamePatient) values ('andres'), ('david'), ('nicky');
insert into Exam (CodeExam, NameExam) values ('v33', 'sid'), ('r55', 'many'), ('d44', 'dieg');

-- Consultas de la prueba
-- Órdenes de hoy
select IdOrder, IdPatient, DateOrder from Orders where DateOrder = CAST(GETDATE() as date);

-- Exámenes de una orden
select E.CodeExam, E.NameExam
from Exam E
inner join OrdersExam OE on OE.IdExam = E.IdExam
where OE.IdOrder = 1;

-- Cantidad de órdenes por paciente (incluye pacientes sin órdenes)
select P.IdPatient, P.NamePatient, count(O.IdOrder) as TotalOrders
from Patient P
left join Orders O on O.IdPatient = P.IdPatient
group by P.IdPatient, P.NamePatient
order by P.IdPatient;