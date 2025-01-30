Import-Module -Name Pode -MaximumVersion 2.99.99
Start-PodeServer {
    Add-PodeEndpoint -Address * -Port 8080 -Protocol Http

    Add-PodeRoute -Method Post -Path '/membercreated' -ScriptBlock {
        $EventData = $WebEvent['Data'] | ConvertFrom-Json
        $Message = ('Member ' + $EventData.data.uniqueName + ' created with an ID of ' + $EventData.data.id)
	    Write-PodeHost $Message
    }
}