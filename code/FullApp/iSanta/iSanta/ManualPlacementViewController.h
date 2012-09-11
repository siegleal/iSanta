//
//  ManualPlacementViewController.h
//  ManualPlacement
//
//  Created by CSSE Department on 1/21/12.
//  Copyright (c) 2012 Rose-Hulman Institute of Technology. All rights reserved.
//

#import <UIKit/UIKit.h>
#import "PlacementBrain.h"
#import "ImageModel.h"
@class DetailViewController;

//@interface mySingleTapRec : UITapGestureRecognizer
//
//
//@end


@interface ManualPlacementViewController : UIViewController
@property (weak, nonatomic) IBOutlet UIImageView *imageView;
@property (nonatomic, strong) PlacementBrain *brain;
@property (nonatomic, strong) ImageModel *images;
@property (strong, nonatomic) IBOutlet UITapGestureRecognizer *tapRecognizer;
@property (strong, nonatomic) IBOutlet UILongPressGestureRecognizer *longPressRec;
@property (strong, nonatomic) IBOutlet UITapGestureRecognizer *singleTapRec;
@property (weak, nonatomic) IBOutlet UIScrollView *scrollView;
@property (weak, nonatomic) IBOutlet UIView *masterView;


@property (nonatomic) DetailViewController *detailView;



@end
