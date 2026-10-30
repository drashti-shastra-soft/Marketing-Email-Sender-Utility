# EmailSender Service

EmailSender Service is a web application for bulk email sending. Users upload an Excel sheet containing recipient data, the records are displayed in a table for review, a template is selected, and the service sends 40–50 emails one by one with email verification before each send. The solution is organized into separate Domain, Infrastructure, Service, and Web Console projects.

## Features

- **Excel upload** — upload an `.xlsx`/`.xls` file containing recipient data (name, email, and any template placeholders).
- **Data preview** — all uploaded rows are parsed and displayed in a table format for review before sending.
- **Row validation** — each row is validated (required fields, email format) and invalid rows are highlighted.
- **Template selection** — choose from available email templates with support for placeholder substitution (e.g., `{{Name}}`, `{{Company}}`).
- **Email verification** — each recipient email is verified (format/syntax, domain/MX check, and optional SMTP or API-based verification) before sending.
- **Bulk sending** — emails are sent one by one (sequentially) in batches of 40–50 with configurable delay/throttling to avoid rate limits.
- **Send status tracking** — per-row status (Pending, Verified, Sent, Failed) with error messages shown in the table.
- **Send summary** — total, sent, failed, and skipped counts after the batch completes.
- **Template management** — create, list, edit, and delete email templates.
- **Upload history** — list of past uploads and their send results.
- **Shared web UI** — Razor views, shared layout, pagination helper, and client-side validation.

> Feature descriptions are based on the controllers, domain models, services, and views present in this repository. Actual behavior may depend on the connected SMTP/email API and database.

## Technology stack

- C# and .NET Framework 4.8 (or `<your target framework>`)
- ASP.NET MVC with Razor views
- Visual Studio solution (`EmailSenderService.sln`)
- Dependency injection configured through Unity
- Entity Framework + SQL Server for storing templates, upload history, and send logs
- Excel parsing via `<EPPlus / ClosedXML / NPOI>`
- Email sending via `<System.Net.Mail.SmtpClient / MailKit / SendGrid / other provider>`
- HTML, CSS, JavaScript, and jQuery-based MVC views

## Solution structure

```text
V4/
├── EmailSenderService.sln
├── EmailSenderService/                    # ASP.NET MVC web application
│   ├── Controllers/                       # MVC request handling
│   ├── Views/                             # Razor UI pages
│   ├── App_Start/                         # Routes, filters, Unity setup
│   ├── Config/                            # Environment-specific app settings
│   ├── Uploads/                           # Uploaded Excel files (if stored locally)
│   └── Web.config
├── EmailSenderService.Domain/             # Domain models and list/pagination models
├── EmailSenderService.Infrastructure/     # Service interfaces, helpers, constants, exceptions
└── EmailSenderService.Service/            # Service implementations, Excel parsing, email sending