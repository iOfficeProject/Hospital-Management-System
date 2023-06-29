create table Payment(
payment_id int primary key identity,
amount int,
payment_method varchar(255),
is_paid bit,
appointment_id int,
user_id int,
FOREIGN KEY (appointment_id) REFERENCES Appointment(appointment_id),
FOREIGN KEY (user_id) REFERENCES Users(user_id)
)