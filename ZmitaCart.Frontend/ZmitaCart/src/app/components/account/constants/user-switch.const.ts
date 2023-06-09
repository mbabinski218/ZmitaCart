export const USER_SWITCHES: UserSwitch[] = [
  {
    name: 'Dane użytkownika',
    value: 'credentials',
  },
  {
    name: 'Czat',
    value: 'chats',
  },
  {
    name: 'Obserwowane',
    value: 'observed',
  },
  {
    name: 'Oferty użytkownika',
    value: 'offers',
  },
  {
    name: 'Kupione',
    value: 'bought',
  },
  {
    name: 'Aktualizacja danych',
    value: 'update',
  },
];

interface UserSwitch {
  name: string,
  value: string,
}