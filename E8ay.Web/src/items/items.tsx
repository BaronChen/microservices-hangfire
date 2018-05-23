import * as React from 'react';
import { IAuctionItem } from './models';
import { connect } from 'react-redux';
import { Dispatch } from 'redux';
import AuctionItemList from './components/auction-item-list';
import { getItemsAction, updateItemInfo, placeBidAction } from './actions/items.actions';
import { getItems } from './reducers';
import { getUserId } from '../login/reducers';
import { IRootState } from '../reducer';

export interface IItemProps {
  items: IAuctionItem[],
  currentUserId: string
}

export class Items extends React.Component<IItemProps & {dispatch: Dispatch}> {

  public componentDidMount(){
    this.props.dispatch(getItemsAction());
  }
  
  public render() {
    const { items } = this.props;

    return (
      <AuctionItemList auctionItems={items}  onBidPriceUpdate={this.updateBidPrice.bind(this)} onPlaceBid={this.onPlaceBid.bind(this)}/>
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