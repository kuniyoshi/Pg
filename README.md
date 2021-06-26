# SUMMARY

![GAME](Documents/play.mov)

# REQUIRES

- DOTween is required
  reference DOTween in asmdef
- UniRx is required
  reference UniRx in asmdef

# TERMS

*TILE*

A hexagon cell.  Tile can contain a gem.

*GEM*

Is in a tile.

When some gems make a cluster as a result of swap operations,
then these gems will vanish.

*CLUSTER*

A group where same colored gems neighbored each other.

*SLIDE*

A phenomenon that gems move to empty tile below.

# SIMULATOR

## FLOW

1. Initialize tile map
2. Request swap operations
3. Receive simulation response
4. Apply simulation response
5. Back to '2. Request swap operations', and loop this cycle until the game is over

## SIMULATION RESPONSE

UI receives simulation response that as a result of the swap operations.
The simulation response contains,

1. Which the tiles did vanish
2. 
