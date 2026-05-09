# 🐝 Polinator: Spatial Decision-Support Platform

## What is Polinator?

Polinator is a web application that **helps beekeepers find the best agricultural plots for placing their beehives**. It combines location data with agricultural information to match beekeepers with suitable plots based on pollination potential.

## What Can Beekeepers Do?

- **View a map** of available agricultural plots in Northern Evoia, Greece
- **Explore plot details** including size, location, crop type, and estimated pollen availability
- **Request a hive placement** by specifying dates and viewing reservation status
- **Track reservations** from initial request through acceptance/rejection to completion
- **Create an account** and log in securely

## What Problems Does It Solve?

1. **Hard to find suitable plots** → Interactive map shows available plots at a glance
2. **Unclear if plots match hive needs** → Plots display pollen information and crop types
3. **No easy way to request placement** → Simple workflow to submit and track requests
4. **Farmers and beekeepers disconnected** → Platform bridges the connection with spatial data

## Who Is This For?

**Beekeepers** who want to place hives on agricultural land and need a way to find and reserve suitable plots.

## Technical Foundation (Brief)

- Built with **React** (frontend), **ASP.NET Core** (backend), and **PostgreSQL with PostGIS** (geospatial database)
- Uses **Leaflet** and **OpenStreetMap** for interactive maps
- Data sourced from Greek open geodata

---

**Status:** Bachelor thesis prototype  
**Geographic Focus:** Northern Evoia, Greece (expandable)