use OrganizatorEvenimente;
go

insert into Vehicule(NrInmatriculare, NrLocuri, Capacitate, Combustibil, Culoare)
	values ('CJ99PWP', 4, 400, 'Diesel', 'CNR');
begin tran
	-- tranzactia 1 -> delay + update + commit
	waitfor delay '00:00:10';
	update Vehicule set Culoare = 'CNRU' where NrInmatriculare = 'CJ99PWP';
	insert into dbo.TransactionLog values('UPDATE', 'Vehicule', GETDATE());
commit tran;