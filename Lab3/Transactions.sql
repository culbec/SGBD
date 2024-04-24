use OrganizatorEvenimente;
go

SET NOCOUNT ON;
go

create table TransactionLog(
	TLid int identity primary key,
	OperationType nvarchar(50),
	TableAffected nvarchar(50),
	ExecutionDate datetime
);
go

create or alter function uf_ValidateVehicleNumber (@vnumber varchar(7)) returns BIT as
begin
	declare @isValid bit = 1;

	if len(@vnumber) = 6
		begin
			if not @vnumber like 'B[0-9][0-9][A-Z][A-Z][A-Z]'
				begin
					set @isValid = 0;
				end
		end
	else if len(@vnumber) = 7
		begin
			if not @vnumber like '[A-Z][A-Z][0-9][0-9][A-Z][A-Z][A-Z]' or
			   @vnumber like 'B[0-9][0-9][0-9][A-Z][A-Z][A-Z]'
				begin
					set @isValid = 0;
				end
		end
	
	if (SELECT COUNT(1) FROM Vehicule where Vehicule.NrInmatriculare = @vnumber) <> 0
		begin
			set @isValid = 0;
		end;

	return @isValid;
end;
go

create or alter function uf_ValidateVehicleCapacity(@vcapacity int) returns BIT as
begin
	declare @isValid bit = 1;
	
	if @vcapacity < 80
		begin
			set @isValid = 0;
		end;

	return @isValid;
end;
go

create or alter function uf_ValidateVehicleColor(@vcolor varchar(10)) returns BIT as
begin
	declare @isValid bit = 1;

	if not @vcolor in ('Red', 'Blue', 'White', 'Black', 'Green')
		begin
			set @isValid = 0;
		end;

	return @isValid;
end;
go

create or alter function uf_ValidateVehicleSeats(@vseats int) returns BIT as
begin
	declare @isValid bit = 1;
	
	if not (@vseats >= 2 and @vseats <= 10)
		begin
			set @isValid = 0;
		end;

	return @isValid;
end;
go

create or alter function uf_ValidateVehicleFuel(@vfuel varchar(20)) returns BIT as
begin
	declare @isValid bit = 1;

	if not @vfuel in ('Diesel', 'Gasoline', 'Electric')
		begin
			set @isValid = 0;
		end;

	return @isValid;
end;
go

create or alter function uf_ValidateProviderName(@pname varchar(100)) returns BIT as
begin
	declare @isValid bit = 1;

	if len(@pname) = 0
		begin
			set @isValid = 0;
		end;
	if (SELECT COUNT(1) FROM Furnizori where Furnizori.Nume = @pname) <> 0
		begin
			set @isValid = 0;
		end;

	return @isValid;
end;
go

create or alter function uf_ValidateProviderSpecialization(@pspec varchar(50)) returns BIT as
begin
	declare @isValid bit = 1;

	if not @pspec in ('Beverages', 'Electronics', 'Audio', 'Visual', 'Sponsor')
		begin
			set @isValid = 0;
		end;

	return @isValid;
end;
go

create or alter procedure usp_AddProvider(
@pname varchar(100), 
@pspec varchar(50),
@fid int output) as
begin
	declare @errors nvarchar(256) = N'';

	BEGIN TRAN
		BEGIN TRY
			if dbo.uf_ValidateProviderName(@pname) <> 1
				begin
					set @errors = @errors +  N' ' +  N'The provider''s name is invalid.';
				end;
			if dbo.uf_ValidateProviderSpecialization(@pspec) <> 1
				begin
					set @errors = @errors + N' ' + N'The providers''s specialization is invalid.';
				end;

			if len(@errors) > 0
				begin
					RAISERROR(@errors, 14, 1);
				end;

			INSERT INTO Furnizori(Nume, Specializare) VALUES (@pname, @pspec);
			INSERT INTO TransactionLog(OperationType, TableAffected, ExecutionDate) VALUES ('Provider Insert', 'Furnizori', GETDATE());
			COMMIT TRAN;
			SELECT @fid = MAX(Fid) from Furnizori;
			print 'Provider insert - committed';
		END TRY

		BEGIN CATCH
			print @errors;
			ROLLBACK TRAN;
			set @fid = -1;
			print 'Provider insert - rollbacked';
		END CATCH
end;
go

create or alter procedure usp_AddVehicle(
@vnumber varchar(7), 
@vcapacity int, 
@vcolor varchar(10), 
@vseats int, 
@vfuel varchar(20),
@vid int output) as 
begin
	declare @errors nvarchar(256) = N'';

	BEGIN TRAN
		BEGIN TRY
			if dbo.uf_ValidateVehicleNumber(@vnumber) <> 1
				begin
					set @errors = @errors + N' ' + N'The vehicle''s number is invalid!';
				end;
			if dbo.uf_ValidateVehicleCapacity(@vcapacity) <> 1
				begin
					set @errors = @errors + N' ' + N'The vehicle''s capacity is invalid!';
				end;
			if dbo.uf_ValidateVehicleColor(@vcolor) <> 1
				begin
					set @errors = @errors + N' ' +  N'The vehicle''s color in invalid!';
				end;
			if dbo.uf_ValidateVehicleSeats(@vseats) <> 1
				begin
					set @errors = @errors + N' ' + N'The vehicle''s seats are invalid!';
				end;
			if dbo.uf_ValidateVehicleFuel(@vfuel) <> 1
				begin
					set @errors = @errors + N' ' + N'The vehicle''s fuels is invalid!';
				end;

			if len(@errors) > 0
				begin
					RAISERROR(@errors, 14, 1);
				end;

			INSERT INTO Vehicule(NrInmatriculare, Capacitate, Culoare, NrLocuri, Combustibil) VALUES (@vnumber, @vcapacity, @vcolor, @vseats, @vfuel);
			INSERT INTO TransactionLog(OperationType, TableAffected, ExecutionDate) VALUES ('Vehicle Insert', 'Vehicule', GETDATE());
			COMMIT TRAN;
			SELECT @vid = MAX(Vid) FROM Vehicule;
			print 'Vehicle insert - committed';
		END TRY

		BEGIN CATCH
			print @errors;
			ROLLBACK TRAN;
			set @vid = -1;
			print 'Vehicle insert - rollbacked'
		END CATCH
end;
go

create or alter procedure usp_AddVehicleProvider(@vid int, @fid int) as
begin
	declare @errors nvarchar(256) = N'';

	BEGIN TRAN
		BEGIN TRY
			if @vid = -1
				begin
					set @errors = @errors + N' ' + N'The vehicle ID is invalid!';
				end;
			if @fid = -1
				begin
					set @errors = @errors + N' ' + N'The provider ID is invalid!';
				end;

			if len(@errors) > 0
				begin
					RAISERROR(@errors, 14, 1);
				end;

			INSERT INTO EvidentaVehicule(Vid, Fid) VALUES (@vid, @fid);
			INSERT INTO TransactionLog(OperationType, TableAffected, ExecutionDate) VALUES ('Vehicle-Provider Insert', 'EvidentaVehicule', GETDATE());
			COMMIT TRAN;
			print 'Vehicle-Provider Insert - committed.';
		END TRY

		BEGIN CATCH
			print @errors;
			ROLLBACK TRAN;
			print 'Vehicle-Provider insert - rollbacked.';
		END CATCH
end;
go

create or alter procedure usp_InsertTran(
	@vnumber varchar(7), 
	@vcapacity int, 
	@vcolor varchar(10), 
	@vseats int, 
	@vfuel varchar(20),
	@pname varchar(100), 
	@pspec varchar(50)) as
begin
	declare @vid int = -1;
	declare @fid int = -1;

	exec dbo.usp_AddVehicle @vnumber, @vcapacity, @vcolor, @vseats, @vfuel, @vid OUTPUT;
	exec dbo.usp_AddProvider @pname, @pspec, @fid OUTPUT;

	exec dbo.usp_AddVehicleProvider @vid, @fid;
end;
go

create or alter procedure usp_InsertTranWhole(
@vnumber varchar(7), 
@vcapacity int, 
@vcolor varchar(10), 
@vseats int, 
@vfuel varchar(20),
@pname varchar(100), 
@pspec varchar(50)) as
begin
	declare @vid int = -1;
	declare @fid int = -1;

	-- first table
	declare @errors nvarchar(256) = N'';
	BEGIN TRAN
		BEGIN TRY
			if dbo.uf_ValidateVehicleNumber(@vnumber) <> 1
				begin
					set @errors = @errors + N' ' + N'The vehicle''s number is invalid!';
				end;
			if dbo.uf_ValidateVehicleCapacity(@vcapacity) <> 1
				begin
					set @errors = @errors + N' ' + N'The vehicle''s capacity is invalid!';
				end;
			if dbo.uf_ValidateVehicleColor(@vcolor) <> 1
				begin
					set @errors = @errors + N' ' +  N'The vehicle''s color in invalid!';
				end;
			if dbo.uf_ValidateVehicleSeats(@vseats) <> 1
				begin
					set @errors = @errors + N' ' + N'The vehicle''s seats are invalid!';
				end;
			if dbo.uf_ValidateVehicleFuel(@vfuel) <> 1
				begin
					set @errors = @errors + N' ' + N'The vehicle''s fuels is invalid!';
				end;

			if len(@errors) > 0
				begin
					set @vid = -1;
					RAISERROR(@errors, 14, 1);
				end;

			INSERT INTO Vehicule(NrInmatriculare, Capacitate, Culoare, NrLocuri, Combustibil) VALUES (@vnumber, @vcapacity, @vcolor, @vseats, @vfuel);
			INSERT INTO TransactionLog(OperationType, TableAffected, ExecutionDate) VALUES ('Vehicle Insert', 'Vehicule', GETDATE());
			SELECT @vid = MAX(Vid) FROM Vehicule;
			print 'Vehicle insert - done';

			set @errors = N'';
			if dbo.uf_ValidateProviderName(@pname) <> 1
				begin
					set @errors = @errors +  N' ' +  N'The provider''s name is invalid.';
				end;
			if dbo.uf_ValidateProviderSpecialization(@pspec) <> 1
				begin
					set @errors = @errors + N' ' + N'The providers''s specialization is invalid.';
				end;

			if len(@errors) > 0
				begin
					set @fid = -1;
					RAISERROR(@errors, 14, 1);
				end;

			INSERT INTO Furnizori(Nume, Specializare) VALUES (@pname, @pspec);
			INSERT INTO TransactionLog(OperationType, TableAffected, ExecutionDate) VALUES ('Provider Insert', 'Furnizori', GETDATE());
			SELECT @fid = MAX(Fid) from Furnizori;
			print 'Provider insert - done'

			set @errors = N'';
			if @vid = -1
				begin
					set @errors = @errors + N' ' + N'The vehicle ID is invalid!';
				end;
			if @fid = -1
				begin
					set @errors = @errors + N' ' + N'The provider ID is invalid!';
				end;

			if len(@errors) > 0
				begin
					RAISERROR(@errors, 14, 1);
				end;

			INSERT INTO EvidentaVehicule(Vid, Fid) VALUES (@vid, @fid);
			INSERT INTO TransactionLog(OperationType, TableAffected, ExecutionDate) VALUES ('Vehicle-Provider Insert', 'EvidentaVehicule', GETDATE());
			COMMIT TRAN;
			print 'Transaction committed.'
		END TRY

		BEGIN CATCH
			print @errors;
			ROLLBACK TRAN;
			print 'Transaction rollbacked.';
		END CATCH
end;
go

create or alter procedure usp_InsertTranBlock(
	@vnumber varchar(7), 
	@vcapacity int, 
	@vcolor varchar(10), 
	@vseats int, 
	@vfuel varchar(20),
	@pname varchar(100), 
	@pspec varchar(50)) as
begin
	declare @vid int = -1;
	declare @fid int = -1;

	-- first table
	declare @errors nvarchar(256) = N'';
	BEGIN TRAN
		BEGIN TRY
			if dbo.uf_ValidateVehicleNumber(@vnumber) <> 1
				begin
					set @errors = @errors + N' ' + N'The vehicle''s number is invalid!';
				end;
			if dbo.uf_ValidateVehicleCapacity(@vcapacity) <> 1
				begin
					set @errors = @errors + N' ' + N'The vehicle''s capacity is invalid!';
				end;
			if dbo.uf_ValidateVehicleColor(@vcolor) <> 1
				begin
					set @errors = @errors + N' ' +  N'The vehicle''s color in invalid!';
				end;
			if dbo.uf_ValidateVehicleSeats(@vseats) <> 1
				begin
					set @errors = @errors + N' ' + N'The vehicle''s seats are invalid!';
				end;
			if dbo.uf_ValidateVehicleFuel(@vfuel) <> 1
				begin
					set @errors = @errors + N' ' + N'The vehicle''s fuels is invalid!';
				end;

			if len(@errors) > 0
				begin
					RAISERROR(@errors, 14, 1);
				end;

			INSERT INTO Vehicule(NrInmatriculare, Capacitate, Culoare, NrLocuri, Combustibil) VALUES (@vnumber, @vcapacity, @vcolor, @vseats, @vfuel);
			INSERT INTO TransactionLog(OperationType, TableAffected, ExecutionDate) VALUES ('Vehicle Insert', 'Vehicule', GETDATE());
			COMMIT TRAN;
			SELECT @vid = MAX(Vid) FROM Vehicule;
			print 'Vehicle insert - committed';
		END TRY

		BEGIN CATCH
			print @errors;
			ROLLBACK TRAN;
			set @vid = -1;
			print 'Vehicle insert - rollbacked';
		END CATCH

	-- second table
	set @errors = N'';
	BEGIN TRAN
		BEGIN TRY
			if dbo.uf_ValidateProviderName(@pname) <> 1
				begin
					set @errors = @errors +  N' ' +  N'The provider''s name is invalid.';
				end;
			if dbo.uf_ValidateProviderSpecialization(@pspec) <> 1
				begin
					set @errors = @errors + N' ' + N'The providers''s specialization is invalid.';
				end;

			if len(@errors) > 0
				begin
					RAISERROR(@errors, 14, 1);
				end;

			INSERT INTO Furnizori(Nume, Specializare) VALUES (@pname, @pspec);
			INSERT INTO TransactionLog(OperationType, TableAffected, ExecutionDate) VALUES ('Provider Insert', 'Furnizori', GETDATE());
			COMMIT TRAN;
			SELECT @fid = MAX(Fid) from Furnizori;
			print 'Provider insert - committed'
		END TRY

		BEGIN CATCH
			print @errors;
			print 'Provider insert - rollbacked';
			ROLLBACK TRAN;
			set @fid = -1;
		END CATCH

		-- link table
		set @errors = N'';
		BEGIN TRAN
		BEGIN TRY
			if @vid = -1
				begin
					set @errors = @errors + N' ' + N'The vehicle ID is invalid!';
				end;
			if @fid = -1
				begin
					set @errors = @errors + N' ' + N'The provider ID is invalid!';
				end;

			if len(@errors) > 0
				begin
					RAISERROR(@errors, 14, 1);
				end;

			INSERT INTO EvidentaVehicule(Vid, Fid) VALUES (@vid, @fid);
			INSERT INTO TransactionLog(OperationType, TableAffected, ExecutionDate) VALUES ('Vehicle-Provider Insert', 'EvidentaVehicule', GETDATE());
			COMMIT TRAN;
			print 'Vehicle-Provider Insert - committed.';
		END TRY

		BEGIN CATCH
			print @errors;
			ROLLBACK TRAN;
			print 'Vehicle-Provider insert - rollbacked.';
		END CATCH
end;
go

exec dbo.usp_InsertTranWhole 'VS66PEP', 100, 'Red', 4, 'Diesel', 'Nutline', 'Beverages';
go

exec dbo.usp_InsertTranWhole 'BZ9999', 100, 'White', 8, 'Electrc', 'Pepsi', 'Beverages';
go

exec dbo.usp_InsertTranBlock 'VS66PEP', 100, 'Red', 4, 'Diesel', 'Nutline', 'Beverages';
go

exec dbo.usp_InsertTranBlock'BZ9999', 100, 'White', 8, 'Electrc', 'Pepsi', 'Beverages';
go

delete from Vehicule where Vehicule.NrInmatriculare = 'VS66PEP';
delete from Furnizori where Furnizori.Specializare = 'Beverages';
go

select * from Vehicule;
select * from Furnizori;
select * from TransactionLog;