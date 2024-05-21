use OrganizatorEvenimente;
go

begin tran
	SET TRANSACTION ISOLATION LEVEL REPEATABLE READ;
	-- tranzactia 2 -> select + delay + select
	select * from Vehicule;
	insert into dbo.TransactionLog values('SELECT', 'Vehicule', GETDATE());

	waitfor delay '00:00:15';

	select * from Vehicule;
	insert into dbo.TransactionLog values('SELECT', 'Vehicule', GETDATE());
commit tran;

insert into dbo.TransactionLog values('Phantom Read', 'Vehicule', GETDATE());