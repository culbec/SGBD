use OrganizatorEvenimente;
go

begin tran
	update Furnizori set Nume = 'DEADN' where Fid = 5;
	waitfor delay '00:00:10'
	update Vehicule set Culoare = 'DEADC' where Vid = 10;
commit tran;