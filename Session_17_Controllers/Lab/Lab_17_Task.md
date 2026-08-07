# 🧪 Lab 17 — Editing a Real Student
## ITI Summer Training | Web Development Using .NET | Morning Group
### Session 17 — Controllers | 3 hours, in the room, graded out of 100

---

# ⚠️⚠️⚠️ READ THIS FIRST — TODAY IS DIFFERENT ⚠️⚠️⚠️

## 🚨 THIS LAB FILE WAS REBUILT FROM SCRATCH TODAY 🚨

## **THIS IS A SMALL, DELIBERATE CONSEQUENCE — NOT AN ACCIDENT, NOT A MISTAKE.**

The room was not genuinely active with Hamdy during today's session — not answering,
not engaging, not present in the way a session like this needs. Hamdy noticed, and he
removed the original Lab 17 file on purpose so that today's lab starts from a slightly
harder place than it would have otherwise.

**This is not a punishment on your grade.** The rubric below is a full, real rubric,
and a good submission still earns full marks. It is a punishment on *comfort* — zero
extra hand-holding today, the task is not softened, and everyone starts the clock
exactly where they would have without the head start today's engagement should have
earned the room.

**Read every word of this file.** Nobody is going to repeat themselves for you today.

---

## 🔑 Your Personal Lab ID

Hamdy calls out a number next to your name on the roster. **Write it down.** Two
concrete values in this lab are derived from it, so no two correct submissions in this
room can be the same file.

Work out both values now, on paper, before you open Visual Studio:

| Value | Formula | Yours |
|---|---|---|
| **MIN_GPA_EDIT** | `2.0 + (Lab ID mod 5) × 0.3` | ______ |
| **MAX_YEAR_EDIT** | `(Lab ID mod 3) + 2` | ______ |

> **Worked example — Lab ID 11.** `11 mod 5 = 1`, so MIN_GPA_EDIT = `2.0 + 0.3` =
> **2.3**. `11 mod 3 = 2`, so MAX_YEAR_EDIT = `2 + 2` = **4**.
> Every code sample in this task uses Lab ID 11's values. **Yours will be different.**
> Copying the samples verbatim is an automatic zero on the affected part.

⚠️ Hamdy will walk around and ask you, out loud, **why your numbers are what they
are**. Have the arithmetic on your paper.

---

## 🎯 What You're Building

You are extending **today's** project — the same `StudentPortalWeb` project from the
lecture, not a new one. By the end you will have built a **second working form**:
editing an existing student, from the empty file up. Both halves of the round trip,
your own validation boundaries, the guard clause, and the redirect — and one action
that carries your own name.

Nothing in this lab needs a concept that was not taught this morning.

---

## ✅ What You Need Before You Start

- Visual Studio, with today's `StudentPortalWeb.slnx` open.
- The project **running** — press F5 once and confirm `/students`, a `Details` page,
  and the `Create` form (both halves) all still work before you change anything.
- Your Student Guide open at Blocks 1 through 5.
- Your Lab ID and your two derived values, on paper.
- SQL Server running. You are not changing any schema today — no migrations, from any
  project. `[Range]` is a validation attribute, not a schema attribute — say why, in a
  comment, when you get to Part D.

---

## Part A — Verify and orient *(15 min, 8 points)*

**Tests:** Block 1 — `IActionResult`, and everything carried forward from Sessions 15–16.

1. Run the app. Confirm `/students`, a `Details` page for a real id, and the `Create`
   form both load the empty form (GET) and save a new row (POST).
2. In a comment at the very top of `StudentsController.cs`, write your **Lab ID** and
   your **two derived values**, in this exact shape:

```csharp
// LAB 17 — Lab ID: 11 | MIN_GPA_EDIT = 2.3 | MAX_YEAR_EDIT = 4
```

3. In the same comment, answer in one sentence: **why is `Create()`'s GET overload not
   marked `async`, even though the POST overload is?**

✅ **Check:** everything that worked yesterday still works before you touch anything.

---

## Part B — The GET half: show the real row *(45 min, 20 points)*

**Tests:** Block 2 — model binding, and the difference between a route value and a
whole bound object.

1. Add an action to `StudentsController`:

```csharp
[HttpGet]
public async Task<IActionResult> Edit(int id)
```

2. Load the matching student from the database. If no row has that id, return the same
   404 helper `Details` already uses.
3. Pass the loaded student to a new view, `Views/Students/Edit.cshtml`, built the same
   way `Create.cshtml` was — a form, one input per editable field, model-bound.
4. The form's fields must arrive **pre-filled** with the real row's current values —
   this is the entire point of loading the student before returning the view.
5. **Record:** visiting `/students/edit/<a real id>` shows that student's actual name,
   year and GPA already sitting in the boxes, not a blank form.

📌 In a comment above the action, answer in one sentence: **why does this action load
the student from the database at all, instead of just rendering an empty form the way
`Create()`'s GET half does?**

✅ **Check:** `/students/edit/9999` (an id that does not exist) returns a 404, not a
blank form and not an exception page.

---

## Part C — The POST half: save the change, properly *(50 min, 22 points)*

**Tests:** Blocks 3 and 5 — two methods with one name, and Post/Redirect/Get.

1. Add the second `Edit` overload:

```csharp
[HttpPost]
public async Task<IActionResult> Edit(int id, Student student)
```

2. Guard clause **first, before any save**: if `id != student.Id`, or if
   `!ModelState.IsValid`, return `View(student)` — the same object handed back, so the
   form redisplays with what was actually typed.
3. If the guard passes, update the row and save. Do **not** insert a new row — this
   must be the same student, changed, not a second student created.
4. On success, set a `TempData` message naming the student, then redirect — **not**
   `return View(...)`. Address the exact reason Block 5 gave for this.

✅ **Check (do this yourself before Hamdy does it for you):** save a real edit, then
press **F5** on the resulting page. The row count in `/students` must not grow. If it
does, you returned a view instead of redirecting, or your guard clause runs after the
save instead of before it — read the order of your own lines.

📌 In a comment above the POST action, answer in one sentence: **what would happen to
this action's signature if you renamed its `id` parameter to `studentId` and left the
route pattern saying `{id}`?**

---

## Part D — Your own validation boundaries *(40 min, 25 points)*

**Tests:** Block 4 — validation attributes belong on the model, and schema vs.
validation attributes are not the same thing.

The `Student` class already carries `[Range(1, 4)]` on `YearOfStudy` and
`[Range(0.0, 4.0)]` on `Gpa`, from this morning's lecture. Today you tighten one of them
to **your own** boundary, derived from your Lab ID — because the point of this part is
proving the rule is actually checked, not just present.

1. Change the `[Range]` attribute on `Gpa` so its **minimum** is your own
   **MIN_GPA_EDIT**, keeping the maximum at `4.0`. Give it your own `ErrorMessage`.
2. Change the `[Range]` attribute on `YearOfStudy` so its **maximum** is your own
   **MAX_YEAR_EDIT**, keeping the minimum at `1`. Give it your own `ErrorMessage`.
3. Run the app. On the `Edit` form for a real student, type a GPA **below** your
   MIN_GPA_EDIT and submit.
4. **Record:** the exact error message shown on screen, and confirm — by checking SSMS
   yourself — that the row in the database was **not** changed.

✅ **Check:** your own MIN_GPA_EDIT itself is **accepted** (the range is inclusive).
Typing exactly your MIN_GPA_EDIT must save successfully; typing one hundredth below it
must not.

📌 In a comment above the `Student` class, answer: **is this a schema attribute or a
validation-only attribute, and how would you prove it to someone who didn't believe
you** — without opening the database?

---

## Part E — One action that carries your own name *(20 min, 15 points)*

**Tests:** Blocks 3 and 5 together — an action is just a public method, and a redirect
is a real instruction to the browser, not a return value you're choosing for style.

1. Instead of redirecting a successful `Edit` to `Index`, add a **new** action named
   after your own first name — for a trainee called Mariam, that is
   `public async Task<IActionResult> MariamConfirmed(int id)`.
2. That action loads the same student by `id` and returns a view (reuse
   `Views/Students/Details.cshtml` by calling `return View("Details", student)`) that
   shows the row **as it now stands in the database** — not the object still sitting in
   memory from the POST.
3. Your successful `Edit` POST redirects to **this** action, passing the student's
   `id`, using `RedirectToAction`, not a plain view return.

✅ **Check:** after saving a change, the browser's address bar shows your confirmation
action's address, not `/Students/Edit`, and pressing F5 there does **not** re-submit
anything.

📌 In a comment above your new action, answer in one sentence: **why must this action
reload the student from the database rather than simply reusing the `student` object
the POST action already had in memory?**

---

## Part F — Wrap-Up Reflection *(10 min, 10 points)*

Create `Reflection_17.md` next to your project and answer all four, about **your own**
values — an answer using the worked example's numbers scores zero.

1. Write out your Lab ID and both derived values, with the arithmetic.
2. Name the three places bad input can be rejected in this project, in the order they
   actually run, and which one your Part D change lives in.
3. You proved in Part C that pressing F5 after a save does not create a duplicate row.
   Explain, in your own words, exactly what changed in the browser to make that true —
   not what you changed in the code, but what the **browser** is now doing differently.
4. Name one thing that is genuinely the same about how `[Required]`/`[MaxLength]` and
   `[Range]` are written, and one thing that is different about what each one actually
   does to the database.

---

## 📋 Grading Rubric

| Part | What earns the marks | Points |
|---|---|---|
| **A** | App verified working before changes (3); Lab ID + two derived values in the header comment (3); comment answered (2) | **8** |
| **B** | `Edit` GET loads the real row and 404s on a bad id (10); form genuinely pre-filled (7); comment answered (3) | **20** |
| **C** | Guard clause correctly placed **before** the save (8); update, not insert (6); redirect, not view — F5-safe (6); comment answered (2) | **22** |
| **D** | Both `[Range]` boundaries use the trainee's own Lab-ID values (10); own MIN_GPA_EDIT itself accepted (5); a bad value genuinely rejected, with real error text recorded (7); comment answered (3) | **25** |
| **E** | Action correctly named after the trainee's own first name (6); reloads from the database rather than reusing the POST's in-memory object (6); redirect wired correctly (3) | **15** |
| **F** | Four reflection answers, using their own values (10) | **10** |
| | **TOTAL** | **100** |

**Automatic deductions:**
- Guard clause placed **after** the save instead of before it: **−10**, even if the
  page happens to look correct.
- `return View(...)` instead of a redirect on a successful POST: **−10**. Hamdy is
  pressing F5 on your machine, not taking your word for it.
- Values copied from the worked example instead of derived from your own Lab ID:
  **zero on that part.**

---

## ⏰ Time Budget

| Part | Minutes |
|---|---|
| A — Verify and orient | 15 |
| B — The GET half | 45 |
| C — The POST half | 50 |
| D — Your own validation boundaries | 40 |
| E — One action, your own name | 20 |
| F — Reflection | 10 |
| **Total** | **180** |

If you are not finished with Part C by the **1:40** mark, tell Hamdy rather than
pushing on — Part D is worth more and needs the time.

---

## 🙋 If You Get Stuck

**Check these four things, in this order, before raising your hand:**

1. **Did you stop the app before re-running?** Shift+F5, then F5. A stale run is the
   number one cause of "my change did nothing."
2. **Is your guard clause genuinely the first line, before any `await _context...` call
   that changes data?** If a bad value still saves, this is almost always why.
3. **Did you press F5 on the page you actually landed on, or on `/Students/Edit`
   directly?** The duplicate-insert bug only shows up on the address the browser is
   really sitting at.
4. **Does your `Edit` POST's `id` parameter name match the `{id}` in its route, and does
   your `Student` parameter's property names match your form's `name` attributes
   exactly?** A mismatch produces no error at all — just a silent `0` or `null`.

Still stuck? Raise your hand, and tell Hamdy **which of those four you have already
checked**. "It doesn't work" is not a question anyone can answer.
