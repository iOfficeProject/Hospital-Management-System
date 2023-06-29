create table Appointment(
appointment_id int primary key identity, 
appointment_date datetime,
appointment_start_time datetime,
appointment_end_time datetime, 
slot_id int,
hospital_id int,
user_id int,
FOREIGN KEY (hospital_id) REFERENCES Hospital(hospital_id),
FOREIGN KEY (slot_id) REFERENCES Slot(slot_id),
FOREIGN KEY (user_id) REFERENCES Users(user_id)
)