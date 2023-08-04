create table Payment(
Payment_Id int primary key identity,
Amount int,
Payment_Method varchar(255),
Is_Paid bit,
Appointment_Id int,
User_Id int,
FOREIGN KEY (Appointment_Id) REFERENCES Appointment(Appointment_Id),
FOREIGN KEY (User_Id) REFERENCES Users(User_Id)
)