export interface Message {
    user: string
    message: string
}

export interface Display {
    id: string
    location: 'ATX' | 'DAL'
    screen: 'left' | 'right'
}

export const displays : Display[] = [
    { id: '1', location: 'ATX', screen: 'left'},
    { id: '2', location: 'ATX', screen: 'right'}
]
