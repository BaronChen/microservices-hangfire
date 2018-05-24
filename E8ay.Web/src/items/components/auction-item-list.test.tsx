jest.mock('lodash', () => ({
  debounce: (fn:any) => (value:any) => {
    fn(value);
  }
}));
import * as React from 'react';
import AuctionItemBlock from './auction-item-block';
import AuctionItemList from './auction-item-list';
import { createShallow } from '@material-ui/core/test-utils';
import { IAuctionItem, ItemStatus } from '../models';

import Grid from '@material-ui/core/Grid';



describe('AuctionItemList tests', () => {
  let shallow: any;

  beforeEach(() => {
    shallow = createShallow({dive: true});
  });

  const getWrapper = (testItem:IAuctionItem[], currentUserId:string, mockUpdateBidFn:any, mockPlaceBidFn:any) => {
    return shallow(<AuctionItemList 
      auctionItems={testItem} 
      currentUserId={currentUserId} 
      onBidPriceUpdate={mockUpdateBidFn} 
      onPlaceBid={mockPlaceBidFn}
      classes={{
        root: 'root'
      }}/>);
  }

  const testItems = [
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

  it('should render item list correctly base on input', () => {

    const mockUpdateBidFn = jest.fn();
    const mockPlaceBidFn = jest.fn();

    const wrapper = getWrapper(testItems, testItems[0].highestBiderId, mockUpdateBidFn, mockPlaceBidFn);
    expect(wrapper.find(Grid).length).toBe(4);
    expect(wrapper.find(AuctionItemBlock).length).toBe(3);
    expect(wrapper.find(AuctionItemBlock).at(0).props().auctionItem).toBe(testItems[0]);
    expect(wrapper.find(AuctionItemBlock).at(0).props().currentUserId).toBe(testItems[0].highestBiderId);
    
    const secondItem = wrapper.find(AuctionItemBlock).at(1);

    const testValue = 200;
    secondItem.props().onBidPriceUpdate(testValue);
    expect(mockUpdateBidFn).toHaveBeenCalledTimes(1);
    expect(mockUpdateBidFn).toHaveBeenCalledWith(testItems[1], testValue);

    secondItem.props().onPlaceBid();
    expect(mockPlaceBidFn).toHaveBeenCalledTimes(1);
    expect(mockPlaceBidFn).toHaveBeenCalledWith(testItems[1]);
  })


  

})