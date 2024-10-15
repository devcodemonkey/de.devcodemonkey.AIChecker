﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using de.devcodemonkey.AIChecker.DataStore.SQLServerEF;

#nullable disable

namespace de.devcodemonkey.AIChecker.DataStore.PostgreSqlEF.Migrations
{
    [DbContext(typeof(AicheckerContext))]
    partial class AicheckerContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("de.devcodemonkey.AIChecker.CoreBusiness.DbModels.Answer", b =>
                {
                    b.Property<Guid>("AnswerId")
                        .HasColumnType("uuid");

                    b.Property<Guid?>("QuestionId")
                        .HasColumnType("uuid");

                    b.Property<string>("Value")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("AnswerId");

                    b.HasIndex(new[] { "QuestionId" }, "IX_Unique_AnswerId")
                        .IsUnique();

                    b.ToTable("Answers");
                });

            modelBuilder.Entity("de.devcodemonkey.AIChecker.CoreBusiness.DbModels.Expected", b =>
                {
                    b.Property<Guid>("ExpectedId")
                        .HasColumnType("uuid");

                    b.Property<string>("Value")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("ExpectedId")
                        .HasName("PK_Expected");

                    b.ToTable("Expecteds");
                });

            modelBuilder.Entity("de.devcodemonkey.AIChecker.CoreBusiness.DbModels.ExpectedsResult", b =>
                {
                    b.Property<Guid>("ExpectedId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("ResultsId")
                        .HasColumnType("uuid");

                    b.HasKey("ExpectedId", "ResultsId");

                    b.ToTable("ExpectedsResults");
                });

            modelBuilder.Entity("de.devcodemonkey.AIChecker.CoreBusiness.DbModels.Img", b =>
                {
                    b.Property<Guid>("ImagesId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("AnswerId")
                        .HasColumnType("uuid");

                    b.Property<byte[]>("Img1")
                        .IsRequired()
                        .HasColumnType("bytea")
                        .HasColumnName("Img");

                    b.HasKey("ImagesId");

                    b.HasIndex("AnswerId");

                    b.ToTable("Img", (string)null);
                });

            modelBuilder.Entity("de.devcodemonkey.AIChecker.CoreBusiness.DbModels.Model", b =>
                {
                    b.Property<Guid>("ModelId")
                        .HasColumnType("uuid");

                    b.Property<string>("BasicModells")
                        .HasColumnType("text");

                    b.Property<string>("Link")
                        .HasColumnType("text");

                    b.Property<double?>("Size")
                        .HasColumnType("double precision");

                    b.Property<string>("Value")
                        .HasColumnType("text");

                    b.HasKey("ModelId");

                    b.ToTable("Models");
                });

            modelBuilder.Entity("de.devcodemonkey.AIChecker.CoreBusiness.DbModels.PromptRatingRound", b =>
                {
                    b.Property<Guid>("PromptRatingRoundId")
                        .HasColumnType("uuid");

                    b.Property<int>("Rating")
                        .HasColumnType("integer");

                    b.Property<Guid>("ResultId")
                        .HasColumnType("uuid");

                    b.Property<int>("Round")
                        .HasColumnType("integer");

                    b.HasKey("PromptRatingRoundId");

                    b.HasIndex("ResultId")
                        .IsUnique();

                    b.ToTable("PromptRatingRounds");
                });

            modelBuilder.Entity("de.devcodemonkey.AIChecker.CoreBusiness.DbModels.Question", b =>
                {
                    b.Property<Guid>("QuestionId")
                        .HasColumnType("uuid");

                    b.Property<string>("Value")
                        .HasColumnType("text");

                    b.HasKey("QuestionId");

                    b.ToTable("Questions");
                });

            modelBuilder.Entity("de.devcodemonkey.AIChecker.CoreBusiness.DbModels.RequestObject", b =>
                {
                    b.Property<Guid>("RequestObjectId")
                        .HasColumnType("uuid");

                    b.Property<string>("Value")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("RequestObjectId");

                    b.ToTable("RequestObjects");
                });

            modelBuilder.Entity("de.devcodemonkey.AIChecker.CoreBusiness.DbModels.RequestReason", b =>
                {
                    b.Property<Guid>("RequestReasonId")
                        .HasColumnType("uuid");

                    b.Property<string>("Value")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("RequestReasonId");

                    b.ToTable("RequestReasons");
                });

            modelBuilder.Entity("de.devcodemonkey.AIChecker.CoreBusiness.DbModels.Result", b =>
                {
                    b.Property<Guid>("ResultId")
                        .HasColumnType("uuid");

                    b.Property<string>("Asked")
                        .HasColumnType("text");

                    b.Property<int>("CompletionTokens")
                        .HasColumnType("integer");

                    b.Property<int>("MaxTokens")
                        .HasColumnType("integer");

                    b.Property<string>("Message")
                        .HasColumnType("text");

                    b.Property<Guid>("ModelId")
                        .HasColumnType("uuid");

                    b.Property<int>("PromptTokens")
                        .HasColumnType("integer");

                    b.Property<Guid?>("QuestionId")
                        .HasColumnType("uuid");

                    b.Property<DateTime>("RequestCreated")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime>("RequestEnd")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("RequestId")
                        .HasColumnType("text");

                    b.Property<Guid>("RequestObjectId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("RequestReasonId")
                        .HasColumnType("uuid");

                    b.Property<DateTime>("RequestStart")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid>("ResultSetId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("SystemPromptId")
                        .HasColumnType("uuid");

                    b.Property<double>("Temperature")
                        .HasColumnType("double precision");

                    b.Property<int>("TotalTokens")
                        .HasColumnType("integer");

                    b.HasKey("ResultId");

                    b.HasIndex("ModelId");

                    b.HasIndex("QuestionId");

                    b.HasIndex("RequestObjectId");

                    b.HasIndex("RequestReasonId");

                    b.HasIndex("ResultSetId");

                    b.HasIndex("SystemPromptId");

                    b.HasIndex(new[] { "ResultId" }, "IX_Unique_ResultSetId");

                    b.ToTable("Results");
                });

            modelBuilder.Entity("de.devcodemonkey.AIChecker.CoreBusiness.DbModels.ResultSet", b =>
                {
                    b.Property<Guid>("ResultSetId")
                        .HasColumnType("uuid");

                    b.Property<string>("PromptRequierements")
                        .HasColumnType("text");

                    b.Property<string>("Value")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("ResultSetId");

                    b.HasIndex(new[] { "ResultSetId" }, "IX_Unique_ResultId");

                    b.ToTable("ResultSets");
                });

            modelBuilder.Entity("de.devcodemonkey.AIChecker.CoreBusiness.DbModels.SystemPrompt", b =>
                {
                    b.Property<Guid>("SystemPromptId")
                        .HasColumnType("uuid");

                    b.Property<string>("Value")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("SystemPromptId")
                        .HasName("PK_SystemPrompt");

                    b.ToTable("SystemPrompts");
                });

            modelBuilder.Entity("de.devcodemonkey.AIChecker.CoreBusiness.DbModels.SystemResourceUsage", b =>
                {
                    b.Property<Guid>("SystemResourceUsageId")
                        .HasColumnType("uuid");

                    b.Property<int>("CpuUsage")
                        .HasColumnType("integer");

                    b.Property<DateTime>("CpuUsageTimestamp")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("GpuMemoryUsage")
                        .HasColumnType("integer");

                    b.Property<DateTime>("GpuMemoryUsageTimestamp")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("GpuUsage")
                        .HasColumnType("integer");

                    b.Property<DateTime>("GpuUsageTimestamp")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("MemoryUsage")
                        .HasColumnType("integer");

                    b.Property<DateTime>("MemoryUsageTimestamp")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("ProcessId")
                        .HasColumnType("integer");

                    b.Property<string>("ProcessName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<Guid>("ResultSetId")
                        .HasColumnType("uuid");

                    b.HasKey("SystemResourceUsageId");

                    b.HasIndex("ResultSetId");

                    b.ToTable("SystemResourceUsage", (string)null);
                });

            modelBuilder.Entity("de.devcodemonkey.AIChecker.CoreBusiness.DbModels.Answer", b =>
                {
                    b.HasOne("de.devcodemonkey.AIChecker.CoreBusiness.DbModels.Question", "Question")
                        .WithOne("Answer")
                        .HasForeignKey("de.devcodemonkey.AIChecker.CoreBusiness.DbModels.Answer", "QuestionId")
                        .HasConstraintName("FK_Answer_Question");

                    b.Navigation("Question");
                });

            modelBuilder.Entity("de.devcodemonkey.AIChecker.CoreBusiness.DbModels.ExpectedsResult", b =>
                {
                    b.HasOne("de.devcodemonkey.AIChecker.CoreBusiness.DbModels.Expected", "Expected")
                        .WithMany("ExpectedsResults")
                        .HasForeignKey("ExpectedId")
                        .IsRequired()
                        .HasConstraintName("FK_ExpectedsResults_Expected");

                    b.HasOne("de.devcodemonkey.AIChecker.CoreBusiness.DbModels.Result", "ExpectedNavigation")
                        .WithMany("ExpectedsResults")
                        .HasForeignKey("ExpectedId")
                        .IsRequired()
                        .HasConstraintName("FK_ExpectedsResults_Result");

                    b.Navigation("Expected");

                    b.Navigation("ExpectedNavigation");
                });

            modelBuilder.Entity("de.devcodemonkey.AIChecker.CoreBusiness.DbModels.Img", b =>
                {
                    b.HasOne("de.devcodemonkey.AIChecker.CoreBusiness.DbModels.Answer", "Answer")
                        .WithMany("Imgs")
                        .HasForeignKey("AnswerId")
                        .IsRequired()
                        .HasConstraintName("FK_Img_Answers");

                    b.Navigation("Answer");
                });

            modelBuilder.Entity("de.devcodemonkey.AIChecker.CoreBusiness.DbModels.PromptRatingRound", b =>
                {
                    b.HasOne("de.devcodemonkey.AIChecker.CoreBusiness.DbModels.Result", "Result")
                        .WithOne("PromptRatingRound")
                        .HasForeignKey("de.devcodemonkey.AIChecker.CoreBusiness.DbModels.PromptRatingRound", "ResultId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Result");
                });

            modelBuilder.Entity("de.devcodemonkey.AIChecker.CoreBusiness.DbModels.Result", b =>
                {
                    b.HasOne("de.devcodemonkey.AIChecker.CoreBusiness.DbModels.Model", "Model")
                        .WithMany("Results")
                        .HasForeignKey("ModelId")
                        .IsRequired()
                        .HasConstraintName("FK_Results_Model");

                    b.HasOne("de.devcodemonkey.AIChecker.CoreBusiness.DbModels.Question", "Question")
                        .WithMany("Results")
                        .HasForeignKey("QuestionId")
                        .HasConstraintName("FK_Results_Question");

                    b.HasOne("de.devcodemonkey.AIChecker.CoreBusiness.DbModels.RequestObject", "RequestObject")
                        .WithMany("Results")
                        .HasForeignKey("RequestObjectId")
                        .IsRequired()
                        .HasConstraintName("FK_Results_RequestObjects");

                    b.HasOne("de.devcodemonkey.AIChecker.CoreBusiness.DbModels.RequestReason", "RequestReason")
                        .WithMany("Results")
                        .HasForeignKey("RequestReasonId")
                        .IsRequired()
                        .HasConstraintName("FK_Results_RequestReasons");

                    b.HasOne("de.devcodemonkey.AIChecker.CoreBusiness.DbModels.ResultSet", "ResultSet")
                        .WithMany("Results")
                        .HasForeignKey("ResultSetId")
                        .IsRequired()
                        .HasConstraintName("FK_Results_ResultSets");

                    b.HasOne("de.devcodemonkey.AIChecker.CoreBusiness.DbModels.SystemPrompt", "SystemPrompt")
                        .WithMany("Results")
                        .HasForeignKey("SystemPromptId")
                        .IsRequired()
                        .HasConstraintName("FK_Results_SystemPrompts");

                    b.Navigation("Model");

                    b.Navigation("Question");

                    b.Navigation("RequestObject");

                    b.Navigation("RequestReason");

                    b.Navigation("ResultSet");

                    b.Navigation("SystemPrompt");
                });

            modelBuilder.Entity("de.devcodemonkey.AIChecker.CoreBusiness.DbModels.SystemResourceUsage", b =>
                {
                    b.HasOne("de.devcodemonkey.AIChecker.CoreBusiness.DbModels.ResultSet", "ResultSet")
                        .WithMany("SystemResourceUsages")
                        .HasForeignKey("ResultSetId")
                        .IsRequired()
                        .HasConstraintName("FK_SystemResourceUsage_ResultSets");

                    b.Navigation("ResultSet");
                });

            modelBuilder.Entity("de.devcodemonkey.AIChecker.CoreBusiness.DbModels.Answer", b =>
                {
                    b.Navigation("Imgs");
                });

            modelBuilder.Entity("de.devcodemonkey.AIChecker.CoreBusiness.DbModels.Expected", b =>
                {
                    b.Navigation("ExpectedsResults");
                });

            modelBuilder.Entity("de.devcodemonkey.AIChecker.CoreBusiness.DbModels.Model", b =>
                {
                    b.Navigation("Results");
                });

            modelBuilder.Entity("de.devcodemonkey.AIChecker.CoreBusiness.DbModels.Question", b =>
                {
                    b.Navigation("Answer");

                    b.Navigation("Results");
                });

            modelBuilder.Entity("de.devcodemonkey.AIChecker.CoreBusiness.DbModels.RequestObject", b =>
                {
                    b.Navigation("Results");
                });

            modelBuilder.Entity("de.devcodemonkey.AIChecker.CoreBusiness.DbModels.RequestReason", b =>
                {
                    b.Navigation("Results");
                });

            modelBuilder.Entity("de.devcodemonkey.AIChecker.CoreBusiness.DbModels.Result", b =>
                {
                    b.Navigation("ExpectedsResults");

                    b.Navigation("PromptRatingRound");
                });

            modelBuilder.Entity("de.devcodemonkey.AIChecker.CoreBusiness.DbModels.ResultSet", b =>
                {
                    b.Navigation("Results");

                    b.Navigation("SystemResourceUsages");
                });

            modelBuilder.Entity("de.devcodemonkey.AIChecker.CoreBusiness.DbModels.SystemPrompt", b =>
                {
                    b.Navigation("Results");
                });
#pragma warning restore 612, 618
        }
    }
}
