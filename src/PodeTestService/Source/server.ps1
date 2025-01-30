Import-Module -Name Pode -MaximumVersion 2.99.99
Start-PodeServer {
    Add-PodeEndpoint -Address * -Port 8080 -Protocol Http

    Add-PodeRoute -Method Post -Path '/Members' -ScriptBlock {
        $Message = ('Member ' + $WebEvent.Data.data.uniqueName + ' created with an ID of ' + $WebEvent.Data.data.id)
        foreach ($key in $WebEvent.Keys) {
            Write-PodeHost "$key - $($WebEvent[$key])"
        }
	    Write-PodeHost $Message
        Write-PodeTextResponse -Value $Message
    }
}