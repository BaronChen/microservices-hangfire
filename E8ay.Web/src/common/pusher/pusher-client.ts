import * as Pusher from 'pusher-js';
import { pusherApiKey, pusherCluster, pusherItemChannel } from '../config';


export const pusher = new Pusher(pusherApiKey, {
  cluster: pusherCluster,
  encrypted: true
});

export const itemChannel = pusher.subscribe(pusherItemChannel);
