//
//  ManualPlacementViewController.m
//  ManualPlacement
//
//  Created by CSSE Department on 1/21/12.
//  Copyright (c) 2012 Rose-Hulman Institute of Technology. All rights reserved.
//

#import "ManualPlacementViewController.h"

@interface ManualPlacementViewController()
@property (nonatomic, strong) NSMutableArray *ivArray;

@end



@implementation ManualPlacementViewController
@synthesize brain = _brain;
@synthesize tapRecognizer = _tapRecognizer;
@synthesize longPressRec = _longPressRec;
@synthesize singleTapRec = _singleTapRec;
@synthesize imageView = _imageView;
@synthesize ivArray = _ivArray;

bool deleting;
const double ALPHAVAL = .4;


int currentOp = 1;

- (void) viewDidLoad
{
    self.imageView.image = self.brain.targetImage;
    self.view.backgroundColor = [UIColor blackColor];
    [self.view addGestureRecognizer:[self tapRecognizer]];
    [self.view addGestureRecognizer:[self longPressRec]];
    [self.view addGestureRecognizer:[self singleTapRec]];
    deleting = NO;
    
}

- (NSArray *)ivArray{
    if (!_ivArray) _ivArray = [[NSMutableArray alloc] init];
    return _ivArray;
}

- (PlacementBrain *)brain
{
    if (!_brain) _brain = [[PlacementBrain alloc] init];
    return _brain;
}


- (IBAction)tapped:(id)sender {
    CGPoint loc = [self.tapRecognizer locationInView:self.view];
    //DEBUG: show the double tap location
    NSLog(@"double tap @ %f %f", loc.x,loc.y);
    //add points to array
    [self.brain addPointatX:loc.x andY:loc.y];
    //draw them 
    UIImageView *iv = [[UIImageView alloc] initWithImage:self.brain.circleImage];
    iv.animationImages = self.brain.animationArray;
    iv.center = loc;
    iv.animationDuration = 1;
    [self.view addSubview:iv];
    //add view to array
    [self.ivArray addObject:iv];
}

- (IBAction)longPress:(id)sender {
    NSLog(@"Long press received");
    //start editing
    deleting = YES;
    
    //dim background
    self.imageView.alpha = ALPHAVAL;
    
    //animate
    for (UIImageView *iv in self.ivArray) {
        [iv startAnimating];
    }
}

- (IBAction)singleTap:(id)sender {
    
}



    /* -(void) touchesBegan:(NSSet *)touches withEvent:(UIEvent *)event
    {
        //Finger Down
        UITouch *anyTouch = [touches anyObject];
        if (anyTouch.tapCount == 1) 
        {
            //Create a new Smile
            NSLog(@"tapped at %f, %f",[self.tapRecognizer locationInView:self.view].x,[self.tapRecognizer locationInView:self.view].y);  
            
        }else if (anyTouch.tapCount == 2) 
        {
            //Create a new Smile
            NSLog(@"twice");        
        }else if (anyTouch.tapCount == 3) 
        {
            //Create a new Smile
            NSLog(@"thrice");        
        }
    }*/

- (UITapGestureRecognizer *) tapRecognizer{
    if (!_tapRecognizer) _tapRecognizer = [[UITapGestureRecognizer alloc] initWithTarget:self action:@selector(tapped:)];
    _tapRecognizer.numberOfTapsRequired = 2;
    return _tapRecognizer;
}





- (BOOL)shouldAutorotateToInterfaceOrientation:(UIInterfaceOrientation)interfaceOrientation
{
    // Return YES for supported orientations
    if ([[UIDevice currentDevice] userInterfaceIdiom] == UIUserInterfaceIdiomPhone) {
        return (interfaceOrientation != UIInterfaceOrientationPortraitUpsideDown);
    } else {
        return YES;
    }
}

- (void)viewDidUnload {
    [self setImageView:nil];
    [self setTapRecognizer:nil];
    [self setLongPressRec:nil];
    [self setSingleTapRec:nil];
    [super viewDidUnload];
}
@end
