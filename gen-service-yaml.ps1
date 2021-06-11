$path = "C:\Users\michael.robertson\source\repos\LeadGenProcessor"

Set-Location -Path $path


$files = Get-ChildItem **\*.csproj -Recurse

foreach ($f in $files){
    $serviceName = (Split-Path $f -Leaf).Replace(".csproj", [System.String]::Empty).Replace(".", "-")
    $proj = ([System.String](Resolve-Path -Path $f.FullName -Relative)).Replace("\", "/").Replace("./", [System.String]::Empty).Trim()
    # Write-Host $serviceName
    # Write-Host $proj
    Write-Host "- name: $($serviceName.ToLower())`n  project: $($proj)`n  env:`n    - name: AppName`n      value: $($serviceName)`n  env_file:`n    - ./pubsub.env`n    - statestore.env"
}
