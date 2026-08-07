# 🧪 Lab 19 — Building the Write Side
## ITI Summer Training | Web Development Using .NET | Morning Group
### Session 19 — Models | 3 hours, in the room, graded out of 100

---

## 🔑 Your Personal Lab ID

Hamdy calls out a number next to your name on the roster. **Write it down.** Two values below
come from it, so no two correct submissions in this room can be the same file.

| Value | Formula | Yours |
|---|---|---|
| **MIN_GRADE_LAB** | `1.0 + (Lab ID mod 4) × 0.5` | ______ |
| **COURSE_COUNT** | `(Lab ID mod 3) + 2` | ______ |

> **Worked example — Lab ID 7.** `7 mod 4 = 3`, so MIN_GRADE_LAB = `1.0 + 1.5` = **2.5**.
> `7 mod 3 = 1`, so COURSE_COUNT = `1 + 2` = **3**. Every sample below uses Lab ID 7's values.
> **Yours will be different.** Copying the samples verbatim is an automatic zero on the affected
> part.

You also need **your own first name**. It becomes a real `Student` row in the database, enrolled
in your own real courses — Hamdy will find it by name in SSMS, no diff tool required.

⚠️ Hamdy will ask you, out loud, **why your MIN_GRADE_LAB is what it is**, and to point at your
own name sitting inside `dbo.Enrollments`.

---

## 🎯 What You're Building

The lecture built the **read** side of `Enrollment` — you can see who's taking what, from either
direction. Today you build the **write** side: a real `EnrollmentsController`, from an empty
file, reusing the guard-clause-then-redirect pattern from Session 17 and the validation-attribute
pattern from today's lecture. **No new concept appears anywhere in this lab that Sessions 17–19
did not already teach.**

---

## ✅ What You Need Before You Start

- Both Visual Studio windows open — today's `StudentPortalWeb.slnx`, AND confirm the Session 14
  console project's migration succeeded (`dbo.Enrollments` exists — check in SSMS).
- The project **running**, with all five of today's lecture TODOs complete: `/students/{id}` shows
  an enrolled-courses table, `/courses` and `/courses/{id}` both work.
- Your Student Guide open at Blocks 2–4.
- Your Lab ID, both derived values, and your first name, on paper.
- `Views/Enrollments/Create.cshtml` is **pre-written for you** — Views were Session 18's taught
  topic, not this lab's. Open it and read it before Part B; it already names the exact fields your
  controller must bind.

---

## Part A — Verify and orient *(15 min, 8 points)*

**Tests:** all five of today's lecture TODOs.

1. Run the app. Confirm `/students/{id}` shows an enrolled-courses table (even if empty for most
   students), and `/courses`/`/courses/{id}` both load.
2. In a comment at the very top of `Controllers/EnrollmentsController.cs` (you create this file in
   Part B), write your Lab ID and both derived values, in this exact shape:

```csharp
// LAB 19 — Lab ID: 7 | MIN_GRADE_LAB = 2.5 | COURSE_COUNT = 3
```

3. In the same comment, answer in one sentence: **why can `CoursesController.Index` use a plain
   `Include(c => c.Enrollments)` with no `ThenInclude`, while `CoursesController.Details` needs
   one?**

✅ **Check:** everything from today's lecture works before you add a single line.

---

## Part B — `EnrollmentsController`: the GET half *(35 min, 18 points)*

**Tests:** Session 15's DI shape, Session 17's `[HttpGet]` empty-form pattern, today's LINQ.

1. Create `Controllers/EnrollmentsController.cs`, empty, from scratch.
2. Add the same field-plus-constructor shape every controller in this project already has.
3. Add:
```csharp
[HttpGet]
public async Task<IActionResult> Create()
```
   It must load ALL Students (ordered by name) and ALL Courses (ordered by name), and pass BOTH
   lists to the view somehow — `ViewData` or `ViewBag`, your choice, since `Views/Enrollments/
   Create.cshtml` already expects two lists under those exact names (read the pre-written view to
   see which). This is the same "load what the form needs to show" idea as any GET-half Create.

✅ **Check:** `/Enrollments/Create` shows a real dropdown of real student names and real course
names — not empty, not placeholder text.

📌 In a comment above the action, answer in one sentence: **why does this action need to query
the database at all, when Session 17's `Create()` GET half for Students needed zero queries?**

---

## Part C — `EnrollmentsController`: the POST half *(45 min, 24 points)*

**Tests:** Session 17's guard-clause-then-redirect pattern, applied to a new entity.

1. Add the second `Create` overload:
```csharp
[HttpPost]
public async Task<IActionResult> Create(Enrollment enrollment)
```
2. Guard clause **first**, before any save: if `!ModelState.IsValid`, return `View(enrollment)` —
   same object handed back, same pattern as Session 17's Student form.
3. If valid: set `EnrollmentDate` to right now (do NOT trust a value the form might send for this —
   set it in the controller, server-side, always), add to `_context.Enrollments`, save
   asynchronously.
4. On success: `TempData` message naming both the student and the course (you'll need to look
   them up, or trust the ones already attached via model binding — think about which is safer),
   then redirect to the enrolled student's OWN details page (`StudentsController.Details`,
   passing their id) — **not** `Enrollments`' own Index, which doesn't exist. Use
   `RedirectToAction`, naming both the action and the controller.

✅ **Check (do this yourself before Hamdy does it for you):** submit a real enrollment, then press
F5 on the resulting page. No duplicate row should appear.

📌 In a comment above the POST action, answer in one sentence: **why is `EnrollmentDate` set in
the controller instead of bound from the form, even though the form technically could include a
hidden field for it?**

---

## Part D — Your own validation boundary *(20 min, 15 points)*

**Tests:** Session 19 Block 2 — validation attributes are reapplied, tightened per-trainee.

The `Enrollment.Grade` property already carries `[Range(0.0, 4.0)]` from the lecture. Today you
tighten its **minimum** to your own **MIN_GRADE_LAB**, keeping the maximum at 4.0.

1. Change the `[Range]` attribute's minimum to your own MIN_GRADE_LAB. Give it your own
   `ErrorMessage`.
2. Run the app. On your `Enrollments/Create` form, submit a grade **below** your MIN_GRADE_LAB
   and confirm it's rejected with your own message, the form re-displaying what you typed.
3. Confirm your own MIN_GRADE_LAB itself is **accepted** (the range is inclusive).
4. Confirm a **completely empty** Grade field (leave it blank) is ALSO accepted — because `Grade`
   is nullable, and `[Range]` never fires on `null`. **Record what happens** when you leave it
   blank versus when you type a number below your bound — they must behave differently, and if
   they don't, you've broken the nullable behaviour Block 3 taught.

✅ **Check:** blank Grade saves successfully with `Grade` genuinely `null` in the database (check
SSMS) — not `0`.

---

## Part E — Enroll yourself, and prove the constraint *(45 min, 25 points)*

**Tests:** Rule 37 fingerprinting, and Block 5's unique-index proof, from your own controller.

1. Using your `Create` form, find-or-add a `Student` row using **your own real first name** as
   `FullName` (if a `Student` with your name doesn't already exist from an earlier lab, add one
   via the existing `/Students/Create` form first — that action is unchanged from Session 17).
2. Enroll that student in **COURSE_COUNT different courses**, using your own form, one at a time.
   Give at least one of them a real grade (anything at or above your MIN_GRADE_LAB) and leave at
   least one ungraded (blank).
3. Visit `/students/{your id}` — confirm all COURSE_COUNT enrollments show, with the graded one
   showing a real coloured `<gpa-badge>` and the ungraded one showing "Not yet graded."
4. **Now try to enroll yourself in the SAME course a second time**, through your own form. Record
   the exact result: does your controller currently show a friendly error, an ugly server error
   page, or something else? (Most trainees' first attempt will NOT handle this gracefully — that's
   expected and graded separately from whether the ROW is actually rejected.)
5. Confirm in SSMS that, regardless of what the PAGE showed, the database itself still has only
   ONE row for that (StudentId, CourseId) pair — the unique index protects the data even if your
   controller's error handling is rough around the edges.

✅ **Check:** your own name, findable in `dbo.Students`, with exactly COURSE_COUNT rows in
`dbo.Enrollments` pointing at it.

📌 In a comment near your Part E work, answer in one sentence: **what real HTTP/database behaviour
did you observe when the duplicate insert was attempted, and does it match what Block 5's console
demo showed?**

---

## Part F — Wrap-Up Reflection *(20 min, 10 points)*

Create `Reflection_19.md` next to your project. Answer all four, about **your own** values.

1. Your Lab ID, MIN_GRADE_LAB and COURSE_COUNT, with the arithmetic.
2. Name the THREE places today's Enrollment data can be rejected before it reaches the database
   (think: client-side, `ModelState`/`[Range]`, and the database itself), and which one your Part
   D change lives in.
3. Block 3's Warm-Up asked how you'd connect Student and Course with just a foreign key. Explain,
   in your own words, in two sentences, why that instinct fails and what actually fixes it.
4. Today's Enrollment uses `Cascade` delete on both its relationships, while Session 14's
   Course→Instructor uses `Restrict`. Pick ONE more relationship from anywhere else in
   StudentPortal (real or one you can imagine — e.g. a future `Assignment` belonging to a
   `Course`) and say which delete behaviour you'd choose and why.

---

## 📋 Grading Rubric

| Part | What earns the marks | Points |
|---|---|---|
| **A** | App verified working (3); Lab ID + both values in a comment (3); comment answered (2) | **8** |
| **B** | Field+constructor shape correct (5); GET loads both real lists (8); comment answered (5) | **18** |
| **C** | Guard clause correctly placed before the save (8); `EnrollmentDate` set server-side (6); redirect (not view) targeting the student's own Details (7); comment answered (3) | **24** |
| **D** | Own MIN_GRADE_LAB used, not copied (6); own bound itself accepted (3); a value below it genuinely rejected with real error text recorded (4); blank Grade behaves differently and correctly (2) | **15** |
| **E** | Own name findable in `dbo.Students` (5); exactly COURSE_COUNT enrollments (8); at least one graded/one ungraded shown correctly (6); duplicate attempt recorded + DB-level protection confirmed in SSMS (6) | **25** |
| **F** | Four reflection answers, using own values (10) | **10** |
| | **TOTAL** | **100** |

**Automatic deductions:**
- `EnrollmentDate` bound from the form instead of set server-side: **−4**.
- `return View(...)` instead of a redirect on a successful POST: **−10**.
- Values copied from the worked example instead of derived from your own Lab ID, or someone
  else's name used instead of your own: **zero on that part.**
- Any change to `StudentsController`, `CoursesController`, or `StudentPortalContext.cs` beyond
  what today's lecture already TODO'd: **−10** — this lab is scoped to one new file plus one
  attribute.

---

## ⏰ Time Budget

| Part | Minutes |
|---|---|
| A — Verify and orient | 15 |
| B — The GET half | 35 |
| C — The POST half | 45 |
| D — Your own validation boundary | 20 |
| E — Enroll yourself, prove the constraint | 45 |
| F — Reflection | 20 |
| **Total** | **180** |

If Part C is not working by the **95-minute** mark, tell Hamdy rather than pushing on — Part E is
worth the most and needs real time.

---

## 🙋 If You Get Stuck

**Check these five, in this order, before raising your hand:**

1. **Did you Shift+F5 before re-running?** A stale run serves old views and old code.
2. **Is your guard clause genuinely the first line inside the POST action, before any
   `_context.Enrollments.AddAsync` call?** If a bad value still saves, this is almost always why.
3. **Does your dropdown's bound property name match `Enrollment`'s real property names exactly**
   (`StudentId`, `CourseId`)? A mismatch produces a silent `0`, not an error.
4. **Confirmed `dbo.Enrollments` actually exists in SSMS** (the Session 14 console project's
   migration succeeded) before blaming your own controller?
5. **Is `Grade`'s input left genuinely EMPTY, not `0`, when you mean "no grade yet"?** Typing `0`
   is a real grade; leaving the field blank is what produces `null`.

Still stuck? Raise your hand and say **which of those five you have already checked.**
