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
const double ALPHAVAL = .25;
const double ANIMATEDURATION = 1;
const int DISTTHRESHOLD = 30;


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
    
    if (!deleting){
        //add points to array
        [self.brain addPointatX:loc.x andY:loc.y];
        //draw them 
        UIImageView *iv = [[UIImageView alloc] initWithImage:self.brain.circleImage];
        iv.animationImages = self.brain.animationArray;
        iv.center = loc;
        iv.animationDuration = ANIMATEDURATION;
        [self.view addSubview:iv];
        //add view to array
        [self.ivArray addObject:iv];
    }
    else
    {
        //done deleting
        deleting = NO;
        
        //undim
        self.imageView.alpha = 1.0;
        
        //stop animating
        for (UIImageView *iv in self.ivArray) {
            [iv stopAnimating];
        }

    }
}

- (IBAction)longPress:(id)sender {
    //start editing
    deleting = YES;
    
    //dim background
    self.imageView.alpha = ALPHAVAL;
    
    //animate
    for (UIImageView *iv in self.ivArray) {
        [iv startAnimating];
    }
    
    //let it be known that I am deleting
    deleting = YES;
}

- (IBAction)singleTap:(id)sender {
    if (deleting)
    {
        CGPoint loc = [self.singleTapRec locationInView:self.view];
        
        //find the closest point
        int closestIndex = 0;
        double closestDist = 999;

        if (self.brain.points.count > 0)
        {
            //loop thru all of the points
            NSEnumerator *e = [self.brain.points objectEnumerator];            
            for (int i = 0; i < self.brain.points.count; i++) 
            {
                NSValue *obj = [e nextObject];
                double dist = sqrt(pow(loc.x - obj.CGPointValue.x,2.0) + pow(loc.y - obj.CGPointValue.y,2.0));
                if (dist < closestDist) {
                    closestDist = dist;
                    closestIndex = i;
                }
            }
            
            //prevent a random tap from removing a point
            if (closestDist < DISTTHRESHOLD){
                //remove from controller & view
                UIImageView *iv = [self.ivArray objectAtIndex:closestIndex];
                [iv removeFromSuperview];
                [self.ivArray removeObjectAtIndex:closestIndex];
                
                
                //remove from brain            
                [self.brain removePoint:[self.brain.points objectAtIndex:closestIndex]];

            }
        }
        

    }
    
}


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
