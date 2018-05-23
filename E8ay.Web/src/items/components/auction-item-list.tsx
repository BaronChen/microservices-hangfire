import * as React from 'react';
import { IAuctionItem } from '../models'
import AuctionItemBlock from './auction-item-block';
import { Theme, WithStyles, withStyles } from '@material-ui/core/styles';
import Grid from '@material-ui/core/Grid';


export interface IAuctionItemListProps {
  auctionItems: IAuctionItem[],
  onBidPriceUpdate: (item:IAuctionItem, bidPrice: number) => void;
  onPlaceBid: (item:IAuctionItem) => void;
}

const decorate = withStyles<"root">((theme: Theme) => ({
  root: {
    padding: "50px"
  }
}));

type PropsWithStyles = IAuctionItemListProps & WithStyles<"root">;

const DecoratedAuctionItemList = decorate(
  class AuctionItemList extends React.Component<PropsWithStyles> {

    public render() {
      const { classes, auctionItems } = this.props;
      const itemBlocks = auctionItems.map( (x:IAuctionItem) => 
        <Grid item={true} xs={12} sm={6} md={3} key={x.id}>
          <AuctionItemBlock auctionItem={x} onBidPriceUpdate={this.onBidPriceUpdate(x).bind(this)} onPlaceBid={this.onPlaceBid(x).bind(this)}/>
        </Grid>
      );

      return (
        <Grid container={true} className={classes.root} spacing={16}>
          {itemBlocks}
        </Grid>
      )
    }

    public const onBidPriceUpdate = (item:IAuctionItem) => (bidPrice:number) => {
      this.props.onBidPriceUpdate(item, bidPrice);
    }

    public const onPlaceBid = (item:IAuctionItem) => () => {
      this.props.onPlaceBid(item);
    }

  }
);

export default DecoratedAuctionItemList;
