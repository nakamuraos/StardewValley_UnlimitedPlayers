using System.Collections.Generic;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;

namespace UnlimitedPlayers.Events.Multiplayer
{
  class ConnectionEvents
  {

    public void PeerConnected(object sender, PeerConnectedEventArgs e)
    {
      List<string> clientMods = new List<string>();
      List<string> denylistMods = new List<string>();
      List<string> allowlistMods = new List<string>();
      List<string> notAllowlistedMods = new List<string>();

      foreach (IMultiplayerPeerMod mod in e.Peer.Mods)
      {
        clientMods.Add(mod.ID);
        if (LazyHelper.ClientModsDenylist.Contains(mod.ID))
          denylistMods.Add(mod.ID);
        if (LazyHelper.ClientModsAllowlist.Contains(mod.ID))
          allowlistMods.Add(mod.ID);
        else if (LazyHelper.ClientModsAllowlist.Count > 0)
          notAllowlistedMods.Add(mod.ID);
      }

      string clientModsStr = string.Join(", ", clientMods);
      string deniedModsStr = string.Join(", ", denylistMods);
      string allowedModsStr = string.Join(", ", allowlistMods);
      string notAllowlistedModsStr = string.Join(", ", notAllowlistedMods);

      if (clientMods.Count > 0) {
        LazyHelper.LogInfo($"Peer {e.Peer.PlayerID} uses mods: {clientModsStr}");
      }
      if (allowlistMods.Count > 0) {
        LazyHelper.LogInfo($"Peer {e.Peer.PlayerID} has allowlisted mods: {allowedModsStr}");
      }
      if (denylistMods.Count > 0) {
        LazyHelper.LogWarn($"Peer {e.Peer.PlayerID} kicked for using illegal mods: {deniedModsStr}");
        Game1.server.kick(e.Peer.PlayerID);
      }
      else if (LazyHelper.ClientModsAllowlist.Count > 0 && notAllowlistedMods.Count > 0) {
        LazyHelper.LogWarn($"Peer {e.Peer.PlayerID} kicked for using non-allowlisted mods: {notAllowlistedModsStr}");
        Game1.server.kick(e.Peer.PlayerID);
      }
    }

  }
}
