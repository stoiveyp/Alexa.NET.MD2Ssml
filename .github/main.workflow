workflow "New workflow" {
  on = "push"
  resolves = ["Test"]
}

action "Build" {
  uses = "actions/setup-dotnet@v1.0.0"
  runs = "dotnet build"
}

action "Test" {
  uses = "actions/setup-dotnet@v1.0.0"
  runs = "dotnet test"
  needs = ["Build"]
}
