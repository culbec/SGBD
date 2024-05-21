use OrganizatorEvenimente;
go

begin tran
	update Vehicule set Culoare = 'DEADC' where Vid = 20;
	waitfor delay '00:00:10'
	update Furnizori set Nume = 'DEADN' where Fid = 5;
commit tran;