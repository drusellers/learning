import {HubConnectionState} from "@microsoft/signalr";

type Props = {
    state?: HubConnectionState
}

export default function ConnectionState({state} : Props) {
    return <div>
        {state}
    </div>
}
