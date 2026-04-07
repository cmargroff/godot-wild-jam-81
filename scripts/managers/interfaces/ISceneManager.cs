using System;
using System.Collections.Generic;
using Godot;

public interface ISceneManager
{
  Dictionary<string, Dictionary<string, Resource>> PreloadedResources { get; set; }
  event Action LoadingShown;
  event Action LoadingHidden;
  void ShowLoading();
  void HideLoading();
  void ChangeScene(string name);
  void ChangeScene(string name, Dictionary<string, Dictionary<string, string>> preloads);
}