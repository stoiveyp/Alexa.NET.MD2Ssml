workflow "New workflow" {
  on = "push"
  resolves = ["Test"]
}

action "Build" {
  uses = "actions/setup-dotnet@d6004ce18bdb4641fec8d84c683b2adb850c3dd5"
  runs = "dotnet build"
}

action "Test" {
  uses = "actions/setup-dotnet@d6004ce18bdb4641fec8d84c683b2adb850c3dd5"
  runs = "dotnet test"
  needs = ["Build"]
}
