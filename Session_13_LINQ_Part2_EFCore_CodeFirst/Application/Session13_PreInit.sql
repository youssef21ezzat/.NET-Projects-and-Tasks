-- =====================================================================
-- Session13_PreInit.sql
-- ITI Summer Training | Web Development Using .NET | Morning Group
-- Session 13 — LINQ Part 2 + EF Core (Code First)
--
-- STYLE GUIDE RULE 32 — Pre-Session Initialization Script.
-- Standalone and IDEMPOTENT: safe to run any number of times, from any
-- starting state, and it always leaves the server in exactly the state
-- Session 13's lecture assumes.
--
-- WHAT THIS SCRIPT DOES, AND WHY:
--
--   1. VERIFIES (never modifies) ITI_StudentPortalDB — the database
--      hand-built with CREATE TABLE in Session 3 and queried in Session
--      4. Block 5 puts it side by side with the EF-generated one for
--      comparison, so it must still exist. This script will NOT create,
--      alter, or drop it; if it is missing, the script says so loudly
--      and you decide what to do.
--
--   2. DROPS ITI_StudentPortalDB_EF if it exists — the database today's
--      Code First migration will CREATE from C# classes. Dropping it
--      guarantees `Update-Database` genuinely builds it live in front of
--      the room rather than silently finding it already there and doing
--      nothing, which would ruin the demo's whole point.
--
-- DESIGN DECISION (flagged explicitly for Hamdy to confirm or change):
--   Today deliberately builds a SEPARATE database rather than pointing
--   Code First at the existing ITI_StudentPortalDB. Two reasons:
--     (a) we never aim a first-time, destructive demo at real prior work;
--     (b) having both databases side by side is the single best teaching
--         comparison available — the same schema, one hand-written in
--         SSMS, one generated from C# classes.
--   If you would rather have EF take over the original database, that is
--   a real and defensible choice, but it changes Block 5's script and
--   needs a different (additive) migration strategy — tell me and I will
--   rebuild it that way rather than you patching it live.
--
-- ⚠️ HONEST LIMITATION: this script was written against Microsoft's
--   T-SQL documentation and reviewed by hand. It has NOT been executed
--   against your actual SQL Server instance, because I have no
--   connection to it from where I work. Run it once, from Chapter 0 of
--   the Instructor Guide, well before the session — not on the morning.
--
-- HOW TO RUN: open in SSMS, connect to the same server used in Sessions
--   3-4, press Execute (F5). Read the Messages pane.
-- =====================================================================

SET NOCOUNT ON;
GO

USE master;
GO

PRINT '=====================================================';
PRINT ' Session 13 PreInit — starting';
PRINT '=====================================================';
GO

-- ---------------------------------------------------------------------
-- STEP 1 — Verify the Session 3/4 reference database still exists.
--          Read-only. This step never modifies anything.
-- ---------------------------------------------------------------------
IF DB_ID(N'ITI_StudentPortalDB') IS NOT NULL
BEGIN
    PRINT 'Session 3/4 reference database ITI_StudentPortalDB : FOUND';
    PRINT '  (left completely untouched — Block 5 compares against it)';
END
ELSE
BEGIN
    PRINT '*** WARNING ***';
    PRINT 'Session 3/4 reference database ITI_StudentPortalDB : NOT FOUND';
    PRINT '  Session 13 will still run correctly — the EF half does not';
    PRINT '  depend on it. What you lose is Block 5''s side-by-side';
    PRINT '  comparison between the hand-written schema and the';
    PRINT '  EF-generated one.';
    PRINT '  To restore it, run Session 3''s (or Session 4''s) PreInit';
    PRINT '  script before this one.';
END
GO

-- ---------------------------------------------------------------------
-- STEP 2 — Drop today's EF target database if a previous run left one.
--          This is what makes the script idempotent: whether you ran it
--          five minutes ago or never, the result is the same clean slate.
--
--          SET SINGLE_USER WITH ROLLBACK IMMEDIATE force-disconnects any
--          session still holding the database open — otherwise the DROP
--          fails with "database is currently in use", which is exactly
--          what happens if Visual Studio still has a connection from an
--          earlier test run.
-- ---------------------------------------------------------------------
IF DB_ID(N'ITI_StudentPortalDB_EF') IS NOT NULL
BEGIN
    PRINT 'Found an existing ITI_StudentPortalDB_EF — dropping it.';

    ALTER DATABASE ITI_StudentPortalDB_EF
        SET SINGLE_USER WITH ROLLBACK IMMEDIATE;

    DROP DATABASE ITI_StudentPortalDB_EF;

    PRINT 'Dropped existing ITI_StudentPortalDB_EF (clean slate for today''s migration demo)';
END
ELSE
BEGIN
    PRINT 'ITI_StudentPortalDB_EF did not exist. Nothing to drop.';
END
GO

-- ---------------------------------------------------------------------
-- STEP 3 — Confirm the clean slate.
-- ---------------------------------------------------------------------
IF DB_ID(N'ITI_StudentPortalDB_EF') IS NULL
BEGIN
    PRINT 'Confirmed: ITI_StudentPortalDB_EF does not exist.';
    PRINT '  Update-Database will create it live during Block 5.';
END
ELSE
BEGIN
    PRINT '*** ERROR ***';
    PRINT 'ITI_StudentPortalDB_EF still exists after the drop attempt.';
    PRINT '  Close Visual Studio and SSMS query windows holding a';
    PRINT '  connection to it, then re-run this script.';
END
GO

PRINT '=====================================================';
PRINT ' PreInit complete. Ready for Session 13.';
PRINT '=====================================================';
GO

-- =====================================================================
-- AFTER THE SESSION
--
-- Session 14 continues in ITI_StudentPortalDB_EF and expects the tables
-- created by today's InitialCreate migration, plus whatever data was
-- seeded. Do NOT re-run this script before Session 14 — it would drop
-- the database today's lecture and lab just built, and Session 14 works
-- by MIGRATING that database forward, not by recreating it.
-- =====================================================================
