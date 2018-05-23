import * as React from 'react';
import { IAuctionItem } from './models';
import { connect } from 'react-redux';
import { Dispatch } from 'redux';
import AuctionItemList from './components/auction-item-list';
import { getItemsAction, updateItemInfo, placeBidAction } from './actions/items.actions';
import { getItems } from './reducers';
import { getUserId } from '../login/reducers';
import { IRootState } from '../reducer';
import { itemChannel } from '../common/pusher/pusher-client';
import { pusherAuctionEndEventName, pusherNewBidEventName} from '../common/config';

export interface IItemProps {
  items: IAuctionItem[],
  currentUserId: string
}

export class Items extends React.Component<IItemProps & {dispatch: Dispatch}> {

  public componentDidMount(){
    this.props.dispatch(getItemsAction());
    itemChannel.bind(pusherAuctionEndEventName, (item:IAuctionItem) => {
      this.props.dispatch(updateItemInfo(item));

    });
    
    itemChannel.bind(pusherNewBidEventName, (item:IAuctionItem) => {
      this.props.dispatch(updateItemInfo(item));
    });
  }

  public componentWillUnmount() {
    itemChannel.unbind(pusherNewBidEventName);
    itemChannel.unbind(pusherAuctionEndEventName);
  }
  
  public render() {
    const { items, currentUserId } = this.props;

    return (
      <AuctionItemList 
        auctionItems={items}  
        currentUserId={currentUserId}
        onBidPriceUpdate={this.updateBidPrice.bind(this)} 
        onPlaceBid={this.onPlaceBid.bind(this)}
      />
    );
  }

  public updateBidPrice(item:IAuctionItem, bidPrice:number) {
    const newItemInfo = Object.assign({}, item, {bidPrice});
    this.props.dispatch(updateItemInfo(newItemInfo));
  }

  public onPlaceBid(item:IAuctionItem) {
    this.props.dispatch(placeBidAction(item.id, this.props.currentUserId, item.bidPrice));
  }
}

const mapStateToProps = (state: IRootState):IItemProps => ({
  items: getItems(state),
  currentUserId: getUserId(state)
});

export default connect(mapStateToProps)(Items);
