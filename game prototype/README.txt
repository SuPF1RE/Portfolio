ETHER SAILS v1 — 2D SPACE TRADING GAME

RUN
Unzip the complete archive and open index.html in a browser. Keep assets/webp beside it. Game logic, CSS and UI are embedded in the HTML; artwork remains as external relative-path WebP images. assets/originals is optional source art, not required to play.

ECONOMY RESTORED
Resource sale prices: Amber 1, Azure crystals 3, Aether 10, Sunstone 25 coins per unit.
Base player gathering rates remain: 1 / 0.8 / 0.6 / 0.4 units per second.
Original first-purchase prices restored: Cargo Hold 12, Beam Harvester 16, Solar Sails 14, Dock Cranes 10, Hire a Worker 24, Worker Sails 20, Worker Harvesters 24.
Repeatable upgrade prices still grow by ceil(base × factor^level). Route unlock prices remain formula-driven: ceil(80 × 5^n) — Azure 80, Nebula Passage 400, Sunstone 2,000, Horizon Harbor 10,000 coins.

PLAY
Workshop tabs: Ship / Crew / Routes. Upgrades can be purchased freely, without quest locks; the shop stays open while time advances. Collect only while stopped over an open planet; cargo sells automatically over an open harbor.
Starting hold: 8 units. Each hold upgrade multiplies capacity by 1.5, rounding down. Each worker holds half the player's capacity, rounding down.
Up to 12 workers; assign Auto or any open planet. Build timers for route openings remain unchanged.

CONTROLS
Mouse/touch: set course. WASD or arrow keys: manual steering. U: Workshop. Space: pause. Esc or ☰: Captain's menu. Reset Game is in the menu.

SAVE DATA
Persistent save key: ether-sails. Game/save version: 1. Compatible v3 progress migrates if no ether-sails save exists. Do not rename the key. Offline income is not implemented.

CONTENTS
Design-notes.md — mechanics, restored prices, formula and refreshed simulation notes.
Quests.csv — timing and balances from the current 3-second-reaction simulation.
assets/webp — seven game sprites/backgrounds. assets/originals — source art for editing.
The standalone 2D preview is available in Arena Live Preview; this ZIP is the downloadable/offline build.