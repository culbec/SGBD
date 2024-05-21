use OrganizatorEvenimente;
go

begin tran
	-- tranzactia 1 -> delay + insert + commit
	waitfor delay '00:00:10';
	insert into Vehicule(NrInmatriculare, NrLocuri, Capacitate, Combustibil, Culoare)
	values ('CJ99PWP', 4, 400, 'Diesel', 'CPR');

	insert into dbo.TransactionLog values('INSERT', 'Vehicule', GETDATE());
commit tran;