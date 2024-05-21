use OrganizatorEvenimente;
go

begin tran
	SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED;
	-- tranzactia 2 -> select + delay + select
	select * from Vehicule;
	insert into dbo.TransactionLog values('SELECT', 'Vehicule', GETDATE());

	waitfor delay '00:00:15';

	select * from Vehicule;
	insert into dbo.TransactionLog values('SELECT', 'Vehicule', GETDATE());
commit tran;

insert into dbo.TransactionLog values('Dirty Read', 'Vehicule', GETDATE());
