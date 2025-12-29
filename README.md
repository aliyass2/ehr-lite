# EHR-Lite (Demo)

EHR-Lite is a lightweight, microservice-based EHR demo built in .NET.  
It focuses on a realistic pattern used in modern healthcare systems: a **BFF (Backend-for-Frontend)** composes a **patient timeline** from multiple domain services (Registry, Encounters, Clinical Notes), with observability via **.NET Aspire / OpenTelemetry**.

This is intentionally **demo-scoped**: static/stub endpoints today, designed to evolve into DB-backed services and event-driven flows later.
