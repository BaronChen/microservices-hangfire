import * as React from 'react';
jest.mock('./actions/items.actions', () => ({
  getItemsAction: jest.fn(), 
  updateItemInfo: jest.fn(), 
  placeBidAction: jest.fn()
}));
jest.mock('../common/pusher/pusher-client', () => ({
  itemChannel: {
    bind: jest.fn(),
    unbind: jest.fn()
  },
}));


import { Items } from './items';
import {shallow} from 'enzyme';
import { ItemStatus, IAuctionItem } from './models';
import { itemChannel } from '../common/pusher/pusher-client';
import AuctionItemList from './components/auction-item-list';
import { getItemsAction, updateItemInfo, placeBidAction } from './actions/items.actions';
import { pusherAuctionEndEventName, pusherNewBidEventName} from '../common/config';




describe("Items container test", () => {
  const testItems:IAuctionItem[] = [
    {
      id: 'test_id',
      name: 'test_name',
      description: 'test_description',
      highestBiderId: 'test_bider_id',
      highestPrice: 12,
      status: ItemStatus.Listed,
      endDateTime: 'test_end_date',
      startDateTime: 'test_start_date',
      bidPrice: 13
    },
    {
      id: 'test_id_2',
      name: 'test_name_2',
      description: 'test_description_2',
      highestBiderId: 'test_bider_id_2',
      highestPrice: 12,
      status: ItemStatus.Listed,
      endDateTime: 'test_end_date',
      startDateTime: 'test_start_date',
      bidPrice: 13
    },
    {
      id: 'test_id_3',
      name: 'test_name_3',
      description: 'test_description_3',
      highestBiderId: 'test_bider_id_3',
      highestPrice: 12,
      status: ItemStatus.Listed,
      endDateTime: 'test_end_date',
      startDateTime: 'test_start_date',
      bidPrice: 13
    }
  ]
  
  it("should render correcly from props", () => {

    const mockDispatch = jest.fn();

    const wrapper = shallow(<Items items={testItems} currentUserId={testItems[0].highestBiderId} dispatch={mockDispatch}/>)

    expect(wrapper.find(AuctionItemList).length).toBe(1);
    expect(mockDispatch).toHaveBeenCalledTimes(1);
    expect(getItemsAction).toHaveBeenCalledTimes(1);

    expect((itemChannel.bind as any).mock.calls[0][0]).toBe(pusherAuctionEndEventName);
    expect((itemChannel.bind as any).mock.calls[1][0]).toBe(pusherNewBidEventName);

    const auctionEndEvent = (itemChannel.bind as any).mock.calls[0][1];
    const newBidEvent = (itemChannel.bind as any).mock.calls[1][1];

    auctionEndEvent();
    expect(mockDispatch).toHaveBeenCalledTimes(2);
    expect(updateItemInfo).toHaveBeenCalledTimes(1);

    newBidEvent();
    expect(mockDispatch).toHaveBeenCalledTimes(3);
    expect(updateItemInfo).toHaveBeenCalledTimes(2);

    const testValue = 300;
    wrapper.find(AuctionItemList).props().onBidPriceUpdate(testItems[0], testValue);
    wrapper.find(AuctionItemList).props().onPlaceBid(testItems[1]);

    expect(mockDispatch).toHaveBeenCalledTimes(5);
    expect(updateItemInfo).toHaveBeenCalledTimes(3);
    expect(updateItemInfo).toHaveBeenLastCalledWith(Object.assign({}, testItems[0], {bidPrice:testValue}));

    expect(placeBidAction).toHaveBeenCalledTimes(1);
    expect(placeBidAction).toHaveBeenLastCalledWith(testItems[1].id, testItems[0].highestBiderId, testItems[1].bidPrice);
  })

})