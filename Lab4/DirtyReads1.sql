use OrganizatorEvenimente;
go

begin tran
	-- tranzactia 1 -> insert + delay + rollback
	insert into Vehicule(NrInmatriculare, NrLocuri, Capacitate, Combustibil, Culoare)
	values ('CJ99PWP', 4, 400, 'Diesel', 'Red');
	waitfor delay '00:00:10';
rollback tran;