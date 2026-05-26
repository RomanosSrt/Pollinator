// Greek regional units (περιφερειακές ενότητες) and their administrative seats
// Source: GeoNames + Wikipedia (May 2026). Coordinates are of the capital city,
// not the regional-unit polygon centroid.
// Format: { region, regionalUnit, capital, lat, lon }

export const greekRegionalUnits = [
  // === ATTICA (8) ===
  { region: "Attica", regionalUnit: "Central Athens",  capital: "Athens",       lat: 37.9838, lon: 23.7275 },
  { region: "Attica", regionalUnit: "North Athens",    capital: "Marousi",      lat: 38.0500, lon: 23.8000 },
  { region: "Attica", regionalUnit: "South Athens",    capital: "Kallithea",    lat: 37.9500, lon: 23.7000 },
  { region: "Attica", regionalUnit: "West Athens",     capital: "Peristeri",    lat: 38.0167, lon: 23.6833 },
  { region: "Attica", regionalUnit: "East Attica",     capital: "Pallini",      lat: 38.0050, lon: 23.8717 },
  { region: "Attica", regionalUnit: "West Attica",     capital: "Elefsina",     lat: 38.0428, lon: 23.5414 },
  { region: "Attica", regionalUnit: "Piraeus",         capital: "Piraeus",      lat: 37.9475, lon: 23.6378 },
  { region: "Attica", regionalUnit: "Islands",         capital: "Piraeus",      lat: 37.9475, lon: 23.6378 },

  // === CENTRAL GREECE (5) ===
  { region: "Central Greece", regionalUnit: "Boeotia",    capital: "Livadeia",  lat: 38.4358, lon: 22.8714 },
  { region: "Central Greece", regionalUnit: "Euboea",     capital: "Chalcis",   lat: 38.4636, lon: 23.6014 },
  { region: "Central Greece", regionalUnit: "Evrytania",  capital: "Karpenisi", lat: 38.9136, lon: 21.7917 },
  { region: "Central Greece", regionalUnit: "Phocis",     capital: "Amfissa",   lat: 38.5278, lon: 22.3789 },
  { region: "Central Greece", regionalUnit: "Phthiotis",  capital: "Lamia",     lat: 38.9000, lon: 22.4333 },

  // === CENTRAL MACEDONIA (7) ===
  { region: "Central Macedonia", regionalUnit: "Imathia",      capital: "Veria",        lat: 40.5239, lon: 22.2042 },
  { region: "Central Macedonia", regionalUnit: "Thessaloniki", capital: "Thessaloniki", lat: 40.6403, lon: 22.9439 },
  { region: "Central Macedonia", regionalUnit: "Kilkis",       capital: "Kilkis",       lat: 40.9947, lon: 22.8744 },
  { region: "Central Macedonia", regionalUnit: "Pella",        capital: "Edessa",       lat: 40.8000, lon: 22.0500 },
  { region: "Central Macedonia", regionalUnit: "Pieria",       capital: "Katerini",     lat: 40.2706, lon: 22.5022 },
  { region: "Central Macedonia", regionalUnit: "Serres",       capital: "Serres",       lat: 41.0911, lon: 23.5481 },
  { region: "Central Macedonia", regionalUnit: "Chalkidiki",   capital: "Polygyros",    lat: 40.3781, lon: 23.4444 },

  // === CRETE (4) ===
  { region: "Crete", regionalUnit: "Heraklion", capital: "Heraklion",       lat: 35.3387, lon: 25.1442 },
  { region: "Crete", regionalUnit: "Lasithi",   capital: "Agios Nikolaos",  lat: 35.1900, lon: 25.7167 },
  { region: "Crete", regionalUnit: "Rethymno",  capital: "Rethymno",        lat: 35.3667, lon: 24.4833 },
  { region: "Crete", regionalUnit: "Chania",    capital: "Chania",          lat: 35.5138, lon: 24.0180 },

  // === EASTERN MACEDONIA AND THRACE (6) ===
  { region: "Eastern Macedonia and Thrace", regionalUnit: "Drama",   capital: "Drama",           lat: 41.1530, lon: 24.1469 },
  { region: "Eastern Macedonia and Thrace", regionalUnit: "Evros",   capital: "Alexandroupoli",  lat: 40.8478, lon: 25.8744 },
  { region: "Eastern Macedonia and Thrace", regionalUnit: "Kavala",  capital: "Kavala",          lat: 40.9389, lon: 24.4042 },
  { region: "Eastern Macedonia and Thrace", regionalUnit: "Thasos",  capital: "Thasos (Limenas)", lat: 40.7806, lon: 24.7167 },
  { region: "Eastern Macedonia and Thrace", regionalUnit: "Xanthi",  capital: "Xanthi",          lat: 41.1411, lon: 24.8889 },
  { region: "Eastern Macedonia and Thrace", regionalUnit: "Rhodope", capital: "Komotini",        lat: 41.1167, lon: 25.4000 },

  // === EPIRUS (4) ===
  { region: "Epirus", regionalUnit: "Arta",       capital: "Arta",        lat: 39.1606, lon: 20.9856 },
  { region: "Epirus", regionalUnit: "Thesprotia", capital: "Igoumenitsa", lat: 39.5042, lon: 20.2658 },
  { region: "Epirus", regionalUnit: "Ioannina",   capital: "Ioannina",    lat: 39.6675, lon: 20.8533 },
  { region: "Epirus", regionalUnit: "Preveza",    capital: "Preveza",     lat: 38.9550, lon: 20.7522 },

  // === IONIAN ISLANDS (5) ===
  { region: "Ionian Islands", regionalUnit: "Zakynthos",  capital: "Zakynthos", lat: 37.7833, lon: 20.9000 },
  { region: "Ionian Islands", regionalUnit: "Corfu",      capital: "Corfu",     lat: 39.6243, lon: 19.9217 },
  { region: "Ionian Islands", regionalUnit: "Ithaca",     capital: "Vathy",     lat: 38.3667, lon: 20.7167 },
  { region: "Ionian Islands", regionalUnit: "Kefalonia",  capital: "Argostoli", lat: 38.1750, lon: 20.4889 },
  { region: "Ionian Islands", regionalUnit: "Lefkada",    capital: "Lefkada",   lat: 38.8333, lon: 20.7000 },

  // === NORTH AEGEAN (5) ===
  { region: "North Aegean", regionalUnit: "Ikaria",  capital: "Agios Kirykos", lat: 37.6111, lon: 26.2972 },
  { region: "North Aegean", regionalUnit: "Lemnos",  capital: "Myrina",        lat: 39.8767, lon: 25.0656 },
  { region: "North Aegean", regionalUnit: "Lesbos",  capital: "Mytilene",      lat: 39.1100, lon: 26.5547 },
  { region: "North Aegean", regionalUnit: "Samos",   capital: "Vathy (Samos)", lat: 37.7547, lon: 26.9772 },
  { region: "North Aegean", regionalUnit: "Chios",   capital: "Chios",         lat: 38.3683, lon: 26.1356 },

  // === PELOPONNESE (5) ===
  { region: "Peloponnese", regionalUnit: "Argolis",   capital: "Nafplio",  lat: 37.5681, lon: 22.8061 },
  { region: "Peloponnese", regionalUnit: "Arcadia",   capital: "Tripoli",  lat: 37.5089, lon: 22.3744 },
  { region: "Peloponnese", regionalUnit: "Corinthia", capital: "Corinth",  lat: 37.9333, lon: 22.9333 },
  { region: "Peloponnese", regionalUnit: "Laconia",   capital: "Sparta",   lat: 37.0744, lon: 22.4297 },
  { region: "Peloponnese", regionalUnit: "Messenia",  capital: "Kalamata", lat: 37.0389, lon: 22.1142 },

  // === SOUTH AEGEAN (13) ===
  { region: "South Aegean", regionalUnit: "Andros",      capital: "Andros",     lat: 37.8333, lon: 24.9333 },
  { region: "South Aegean", regionalUnit: "Kalymnos",    capital: "Kalymnos",   lat: 36.9500, lon: 26.9833 },
  { region: "South Aegean", regionalUnit: "Karpathos",   capital: "Pigadia",    lat: 35.5061, lon: 27.2125 },
  { region: "South Aegean", regionalUnit: "Kea-Kythnos", capital: "Ioulida",    lat: 37.6411, lon: 24.3267 },
  { region: "South Aegean", regionalUnit: "Kos",         capital: "Kos",        lat: 36.8939, lon: 27.2881 },
  { region: "South Aegean", regionalUnit: "Milos",       capital: "Plaka",      lat: 36.7325, lon: 24.4244 },
  { region: "South Aegean", regionalUnit: "Mykonos",     capital: "Mykonos",    lat: 37.4467, lon: 25.3289 },
  { region: "South Aegean", regionalUnit: "Naxos",       capital: "Naxos",      lat: 37.1042, lon: 25.3761 },
  { region: "South Aegean", regionalUnit: "Paros",       capital: "Paroikia",   lat: 37.0858, lon: 25.1500 },
  { region: "South Aegean", regionalUnit: "Rhodes",      capital: "Rhodes",     lat: 36.4344, lon: 28.2178 },
  { region: "South Aegean", regionalUnit: "Syros",       capital: "Ermoupoli",  lat: 37.4467, lon: 24.9433 },
  { region: "South Aegean", regionalUnit: "Thira",       capital: "Fira",       lat: 36.4167, lon: 25.4333 },
  { region: "South Aegean", regionalUnit: "Tinos",       capital: "Tinos",      lat: 37.5378, lon: 25.1622 },

  // === THESSALY (5) ===
  { region: "Thessaly", regionalUnit: "Karditsa", capital: "Karditsa", lat: 39.3636, lon: 21.9211 },
  { region: "Thessaly", regionalUnit: "Larissa",  capital: "Larissa",  lat: 39.6390, lon: 22.4191 },
  { region: "Thessaly", regionalUnit: "Magnesia", capital: "Volos",    lat: 39.3608, lon: 22.9417 },
  { region: "Thessaly", regionalUnit: "Sporades", capital: "Skopelos", lat: 39.1228, lon: 23.7269 },
  { region: "Thessaly", regionalUnit: "Trikala",  capital: "Trikala",  lat: 39.5556, lon: 21.7675 },

  // === WESTERN GREECE (3) ===
  { region: "Western Greece", regionalUnit: "Achaea",            capital: "Patras",     lat: 38.2466, lon: 21.7346 },
  { region: "Western Greece", regionalUnit: "Aetolia-Acarnania", capital: "Mesolongi",  lat: 38.3722, lon: 21.4286 },
  { region: "Western Greece", regionalUnit: "Elis",              capital: "Pyrgos",     lat: 37.6747, lon: 21.4406 },

  // === WESTERN MACEDONIA (4) ===
  { region: "Western Macedonia", regionalUnit: "Florina",  capital: "Florina",  lat: 40.7861, lon: 21.4097 },
  { region: "Western Macedonia", regionalUnit: "Grevena",  capital: "Grevena",  lat: 40.0833, lon: 21.4333 },
  { region: "Western Macedonia", regionalUnit: "Kastoria", capital: "Kastoria", lat: 40.5167, lon: 21.2667 },
  { region: "Western Macedonia", regionalUnit: "Kozani",   capital: "Kozani",   lat: 40.3017, lon: 21.7886 },
];

// Sanity check: should be 74
// console.log(greekRegionalUnits.length); // 74