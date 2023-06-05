export const USER_SWITCHES: UserSwitch[] = [
  {
    name: 'Dane użytkownika',
    value: 'credentials',
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
    name: 'Aktualizacja adresu',
    value: 'update',
  },
];

interface UserSwitch {
  name: string,
  value: string,
}