use OrganizatorEvenimente;
go

select * from Vehicule;
select * from Furnizori;
select * from EvidentaVehicule;
go

-- Dirty Reads
create or alter procedure DirtyReads as
begin
	begin tran
		-- tranzactia 1 -> insert + delay + rollback
		insert into Vehicule(NrInmatriculare, NrLocuri, Capacitate, Combustibil, Culoare)
		values ('CJ99PWP', 4, 400, 'Diesel', 'Red');
		waitfor delay '00:00:10';

	rollback tran;

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
end;
go

create or alter procedure DirtyReads_Fix as
begin
	begin tran
		-- tranzactia 1 -> insert + delay + rollback
		insert into Vehicule(NrInmatriculare, NrLocuri, Capacitate, Combustibil, Culoare)
		values ('CJ99PWP', 4, 400, 'Diesel', 'Red');

		waitfor delay '00:00:10';

	rollback tran;

	begin tran
		SET TRANSACTION ISOLATION LEVEL READ COMMITTED;
		-- tranzactia 2 -> select + delay + select
		select * from Vehicule;
		insert into dbo.TransactionLog values('SELECT', 'Vehicule', GETDATE());

		waitfor delay '00:00:15';

		select * from Vehicule;
		insert into dbo.TransactionLog values('SELECT', 'Vehicule', GETDATE());

	commit tran;

	insert into dbo.TransactionLog values('Dirty Read Fix', 'Vehicule', GETDATE());
end;
go

-- Non Repeatable Reads
create or alter procedure NonRepeatableReads as
begin
	insert into Vehicule(NrInmatriculare, NrLocuri, Capacitate, Combustibil, Culoare)
	values ('CJ99PWP', 4, 400, 'Diesel', 'CNR');
	begin tran
		-- tranzactia 1 -> delay + update + commit
		
		waitfor delay '00:00:10';
		update Vehicule set Culoare = 'CNRU' where NrInmatriculare = 'CJ99PWP';
		insert into dbo.TransactionLog values('UPDATE', 'Vehicule', GETDATE());

	commit tran;

	begin tran
		SET TRANSACTION ISOLATION LEVEL READ COMMITTED;
		-- tranzactia 2 -> select + delay + select
		select * from Vehicule;
		insert into dbo.TransactionLog values('SELECT', 'Vehicule', GETDATE());

		waitfor delay '00:00:15';

		select * from Vehicule;
		insert into dbo.TransactionLog values('SELECT', 'Vehicule', GETDATE());

	commit tran;

	insert into dbo.TransactionLog values('Non Repeatable Read', 'Vehicule', GETDATE());
end;
go

create or alter procedure NonRepeatableReads_Fix as
begin
	insert into Vehicule(NrInmatriculare, NrLocuri, Capacitate, Combustibil, Culoare)
	values ('CJ99PWP', 4, 400, 'Diesel', 'CNR');
	begin tran
		-- tranzactia 1 -> delay + update + commit
		
		waitfor delay '00:00:10';
		update Vehicule set Culoare = 'CNRU' where NrInmatriculare = 'CJ99PWP';
		insert into dbo.TransactionLog values('UPDATE', 'Vehicule', GETDATE());

	commit tran;

	begin tran
		SET TRANSACTION ISOLATION LEVEL REPEATABLE READ;
		-- tranzactia 2 -> select + delay + select
		select * from Vehicule;
		insert into dbo.TransactionLog values('SELECT', 'Vehicule', GETDATE());

		waitfor delay '00:00:15';

		select * from Vehicule;
		insert into dbo.TransactionLog values('SELECT', 'Vehicule', GETDATE());

	commit tran;

	insert into dbo.TransactionLog values('Non Repeatable Read Fix', 'Vehicule', GETDATE());
end;
go

-- Phantom Reads
create or alter procedure PhantomReads as
begin
	begin tran
		-- tranzactia 1 -> delay + insert + commit
		
		waitfor delay '00:00:10';
		insert into Vehicule(NrInmatriculare, NrLocuri, Capacitate, Combustibil, Culoare)
		values ('CJ99PWP', 4, 400, 'Diesel', 'CPR');

		insert into dbo.TransactionLog values('INSERT', 'Vehicule', GETDATE());

	commit tran;

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
end;
go

create or alter procedure PhantomRead_Fix as
begin
	begin tran
		-- tranzactia 1 -> delay + insert + commit
		
		waitfor delay '00:00:10';
		insert into Vehicule(NrInmatriculare, NrLocuri, Capacitate, Combustibil, Culoare)
		values ('CJ99PWP', 4, 400, 'Diesel', 'CPR');

		insert into dbo.TransactionLog values('INSERT', 'Vehicule', GETDATE());

	commit tran;

	begin tran
		SET TRANSACTION ISOLATION LEVEL SERIALIZABLE;
		-- tranzactia 2 -> select + delay + select
		select * from Vehicule;
		insert into dbo.TransactionLog values('SELECT', 'Vehicule', GETDATE());

		waitfor delay '00:00:15';

		select * from Vehicule;
		insert into dbo.TransactionLog values('SELECT', 'Vehicule', GETDATE());

	commit tran;

	insert into dbo.TransactionLog values('Phantom Read Fix', 'Vehicule', GETDATE());
end;
go

-- Deadlock

create or alter procedure Deadlock as 
begin
	begin tran
		update Vehicule set Culoare = 'DEADC' where Vid = 20;
		waitfor delay '00:00:10'
		update Furnizori set Nume = 'DEADN' where Fid = 5;
	commit tran;

	begin tran
		update Furnizori set Nume = 'DEADN' where Fid = 5;
		waitfor delay '00:00:10'
		update Vehicule set Culoare = 'DEADC' where Vid = 20;
	commit tran;
end;
go

create or alter procedure Deadlock_Fix1 as
begin
	begin tran
		update Vehicule set Culoare = 'DEADC' where Vid = 20;
		waitfor delay '00:00:10'
		update Furnizori set Nume = 'DEADN' where Fid = 5;
	commit tran;

	begin tran
		update Vehicule set Culoare = 'DEADC' where Vid = 20;
		waitfor delay '00:00:10'
		update Furnizori set Nume = 'DEADN' where Fid = 5;
	commit tran;
end;
go

create or alter procedure Deadlock_Fix2 as
begin
	begin tran
		update Vehicule set Culoare = 'DEADC' where Vid = 20;
		waitfor delay '00:00:10'
		update Furnizori set Nume = 'DEADN' where Fid = 5;
	commit tran;

	begin tran
		SET DEADLOCK_PRIORITY HIGH;

		update Furnizori set Nume = 'DEADN' where Fid = 5;
		waitfor delay '00:00:10'
		update Vehicule set Culoare = 'DEADC' where Vid = 20;
	commit tran;
end;
go

create or alter procedure UpdateVehFur as
begin
	begin tran
		update Vehicule set Culoare = 'DEADC' where Vid = 20;
		waitfor delay '00:00:10'
		update Furnizori set Nume = 'DEADN' where Fid = 5;
	commit tran;
end;
go

create or alter procedure UpdateFurVeh as
begin
	begin tran
		update Furnizori set Nume = 'DEADN' where Fid = 5;
		waitfor delay '00:00:10'
		update Vehicule set Culoare = 'DEADC' where Vid = 20;
	commit tran;
end;
go