`0.4.0`
- fixed for sots
- Custodian's flow now plays a unique jingle based on each stage
    - huge thanks to NAIRB for the idea and for creating all the jingles!
- Added a volume slider for Flow jingles, use Risk of Options to see it
- Added config to mute Flow jingles completely
- fixed flow jingle persisting between stages
- heavily reduced attack speed slow on gravity'd enemies
- reduced hitstop on m1, hopefully combatting the amount of times an air jump is eaten during hitstop
- buffed spike impact damage per meter fell (0.32 -> 0.36), spike impact now has sweetspot falloff
    - spike configs were reset
    - *damage was conservative because spiking a group of enemies would stack way too quickly. now with damage falloff each spike can afford to do more damage*
    - *4% might not seem like much but it's for every meter fallen. this could get crazy. let me know how it feels*
- Added Blast Boot (wip, disabled for now)
- Fabinhoru's dagger fixed to its actual description
    - no longer stacks on every hit. now applies [itemcount] amount of stacks, and repeated hits just refresh this stack amount.
    - also stacks were previously only increasing duration, which was dumb
- removed giant portal from enigmatic keycard projectile spawn, toned down impact effect
- decoupled english from language file. damage numbers and stuff should update based on config now (after restart)

`0.3.3`
- null checks galore. hopefully fixed errors from glass harvester, enigmatic keycard, and fabinhoru's dagger
- fixed bismuth earrings not properly adding barrier on applying bleed
- added Brazilian Portuguese translation (thanks Kauzok!)

`0.3.2`
- all items are now in their proper tiers
  - including broken glass harvester, removing it from the item pool
- added per-item configs

`0.3.1`
- added animations for MAID
- added config to disable jingle when activating flow
- Bismuth Earrings reworked
- Glass harvester reworked
- fixed wyatt becoming eldritch aboniation when emoting
- fixed issues with fabinhoru's dagger visuals
- fixed error caused fabinhoru's dagger and/or enigmatic keycard

`0.3.0`
- redid the whole fuckin thing.