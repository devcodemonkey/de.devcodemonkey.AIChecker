delete from Results
delete from Models
delete from SystemPromts
delete from SystemResourceUsage
delete from ResultSets

select * from Questions
select * from Answers
select * from Results order by RequestStart desc
select * from Models
select * from ResultSets
select * from SystemPromts
select * from SystemResourceUsage
--where ProcessId = 26964
	order by GpuUsage desc, MemoryUsage desc
