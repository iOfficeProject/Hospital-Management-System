create table Slot( 
slot_id int primary key identity,
slot_date datetime,
slot_start_time datetime,
slot_end_time datetime,
availability_id int,
hospital_id int,
user_id int,
FOREIGN KEY (availability_id) REFERENCES Availability(availability_id),
FOREIGN KEY (hospital_id) REFERENCES Hospital(hospital_id),
FOREIGN KEY (user_id) REFERENCES Users(user_id)
)