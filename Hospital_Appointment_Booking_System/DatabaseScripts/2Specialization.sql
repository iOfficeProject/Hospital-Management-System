create table Specialization(
specialization_id int primary key identity,
specialization_name varchar(255),
hospital_id int,
FOREIGN KEY (hospital_id) REFERENCES Hospital(hospital_id)
)