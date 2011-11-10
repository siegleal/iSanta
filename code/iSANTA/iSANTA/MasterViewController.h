//
//  MasterViewController.h
//  iSANTA
//
//  Created by Jack Hall on 10/21/11.
//  Copyright (c) 2011 __MyCompanyName__. All rights reserved.
//

#import <UIKit/UIKit.h>

@class DetailViewController;

@interface MasterViewController : UITableViewController

@property (nonatomic) BOOL editing;
@property (retain, nonatomic) UIBarButtonItem *editButton;
@property (retain, nonatomic) UIBarButtonItem *cancelButton;

-(IBAction)edit:(id)sender;
-(IBAction)cancel:(id)sender;


@end
