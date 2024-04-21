import {displays} from "../lib/api";

type Props = {
    display: string,
    onChange: (value: string) => void
}
export default function ScreenSelector({display, onChange} : Props ){
    return <select onChange={(evt) => onChange(evt.target.value)} value={display}>
        {displays.map(d => {
            return <option key={d.id} value={d.id}>
                {d.location} : {d.screen}
            </option>
        })}
    </select>
}
