jest.mock('lodash', () => ({
  debounce: (fn:any) => (value:any) => {
    fn(value);
  }
}));
import * as React from 'react';
import AuctionItemBlock from './auction-item-block';
import { createShallow } from '@material-ui/core/test-utils';
import { IAuctionItem, ItemStatus } from '../models';
// import {shallow} from 'enzyme';

import {Countdown} from '../../common/countdown/coutdown.component';
import CardActions from '@material-ui/core/CardActions';
import Typography from '@material-ui/core/Typography';
import Button from '@material-ui/core/Button';
import TextField from '@material-ui/core/TextField';


describe('AuctionItemBlock tests', () => {
  let shallow: any;

  beforeEach(() => {
    shallow = createShallow({dive: true});
  });

  const getWrapper = (testItem:IAuctionItem, currentUserId:string, mockUpdateBidFn:any, mockPlaceBidFn:any) => {
    return shallow(<AuctionItemBlock 
      auctionItem={testItem} 
      currentUserId={currentUserId} 
      onBidPriceUpdate={mockUpdateBidFn} 
      onPlaceBid={mockPlaceBidFn}
      classes={{
        card: 'card',
        pos: 'pos',
        textField:'textField'
      }}/>);
  }

  it('should render item correctly base on input', () => {
    const testItem: IAuctionItem = {
      id: 'test_id',
      name: 'test_name',
      description: 'test_description',
      highestBiderId: 'test_bider_id',
      highestPrice: 12,
      status: ItemStatus.Listed,
      endDateTime: 'test_end_date',
      startDateTime: 'test_start_date',
      bidPrice: 13
    }

    const mockUpdateBidFn = jest.fn();
    const mockPlaceBidFn = jest.fn();

    const wrapper = getWrapper(testItem, testItem.highestBiderId, mockUpdateBidFn, mockPlaceBidFn);
    expect(wrapper.find(Typography).length).toBe(8);
    expect(wrapper.find(Typography).at(0).childAt(0).text()).toBe(testItem.name);
    expect(wrapper.find(Typography).at(1).childAt(0).text()).toContain(testItem.highestPrice);
    expect(wrapper.find(Typography).at(2).childAt(0).childAt(0).text()).toBe("No offer yet.");
    expect(wrapper.find(Typography).at(3).childAt(0).text()).toBe(testItem.description);
    expect(wrapper.find(Countdown).length).toBe(1);
    expect(wrapper.find(Countdown).props().dateStr).toBe(testItem.endDateTime);
    expect(wrapper.find(CardActions).length).toBe(1);

    const textField = wrapper.find(TextField);
    expect(textField.length).toBe(1);

    const testValue = 100;
    textField.props().onChange({target:{value:testValue}});
    expect(mockUpdateBidFn).toHaveBeenCalledTimes(1);
    expect(mockUpdateBidFn).toHaveBeenCalledWith(testValue);

    const bidButton = wrapper.find(Button);
    expect(bidButton.length).toBe(1);
    bidButton.props().onClick();
    expect(mockPlaceBidFn).toHaveBeenCalledTimes(1);

  })

  it('should show correct message when item status is UnderOffer', () => {
    const testItem: IAuctionItem = {
      id: 'test_id',
      name: 'test_name',
      description: 'test_description',
      highestBiderId: 'test_bider_id',
      highestPrice: 12,
      status: ItemStatus.UnderOffer,
      endDateTime: 'test_end_date',
      startDateTime: 'test_start_date',
      bidPrice: 13
    }

    const mockUpdateBidFn = jest.fn();
    const mockPlaceBidFn = jest.fn();

    const wrapper = getWrapper(testItem, testItem.highestBiderId, mockUpdateBidFn, mockPlaceBidFn);
    expect(wrapper.find(Typography).at(2).childAt(0).childAt(0).text()).toBe("You are the highest bidder for now.");

    
    const wrapper2 = getWrapper(testItem, "otherId", mockUpdateBidFn, mockPlaceBidFn);
    expect(wrapper2.find(Typography).at(2).childAt(0).childAt(0).text()).toBe("Currently under offer.");
  })

  it('should show correct message when item status is Sold or End', () => {
    const soldItem: IAuctionItem = {
      id: 'test_id',
      name: 'test_name',
      description: 'test_description',
      highestBiderId: 'test_bider_id',
      highestPrice: 12,
      status: ItemStatus.Sold,
      endDateTime: 'test_end_date',
      startDateTime: 'test_start_date',
      bidPrice: 13
    }

    const endItem: IAuctionItem = {
      id: 'test_id',
      name: 'test_name',
      description: 'test_description',
      highestBiderId: '',
      highestPrice: 0,
      status: ItemStatus.End,
      endDateTime: 'test_end_date',
      startDateTime: 'test_start_date',
      bidPrice: 0
    }

    const mockUpdateBidFn = jest.fn();
    const mockPlaceBidFn = jest.fn();

    const soldWrapper = getWrapper(soldItem, soldItem.highestBiderId, mockUpdateBidFn, mockPlaceBidFn);
    expect(soldWrapper.find(Typography).at(2).childAt(0).childAt(0).text()).toBe("You are the highest bidder for now.");
    expect(soldWrapper.find(Typography).at(8).childAt(0).text()).toBe("Item has been sold");
    expect(soldWrapper.find(CardActions).length).toBe(0);

    
    const endWrapper = getWrapper(endItem, endItem.highestBiderId, mockUpdateBidFn, mockPlaceBidFn);
    expect(endWrapper.find(Typography).at(8).childAt(0).text()).toBe("Auction has ended");
    expect(endWrapper.find(CardActions).length).toBe(0);

  })

  

})