import React, { Component } from 'react'
import { Container, Row, Col } from 'reactstrap';
import Note from './Note'
import NoteInput from './NoteInput'
import { getNotes, createNote } from './Fetch'
import '../styles/styles.css'

export default class Notes extends Component {
    constructor (props) {
        super(props);
        this.state = { notes: [], loading: true};

        this.last = React.createRef();
    }

    renderNotes = (notes) => {
        return (
            <div>
                {notes.map(n => (
                    <Note 
                        key={n.id} 
                        id={n.id} 
                        text={n.text}
                        date={n.dateModified}/>
                ))}
            </div>
        )
    }

    createNew = async (text) => {
        const result = await createNote(text);
        if (result === 204)
            await this.fetchNotes();
        else
            this.props.history.push('/500')
    }

    fetchNotes = async () => {
        const {obj, status} = await getNotes();
        if (status === 200)
            this.setState({notes: obj, loading: false});
        else 
            this.props.history.push('/500')
    }

    async componentDidMount() {
        await this.fetchNotes();
        if (this.last.current)
            this.last.current.scrollIntoView({
                behavior: 'smooth',
                block: 'start'
            })
    }

    componentDidUpdate() {
        if (this.last.current)
            this.last.current.scrollIntoView({
                behavior: 'smooth',
                block: 'start'
            })
    }

    render(){
        const contents = this.state.loading
            ? <p><em>Loading...</em></p>
            : this.renderNotes(this.state.notes)

        return (      
            <Container ref={this.cont}>  
                <Row>   
                    <Col className="note-container">
                        {contents}
                        <div ref={this.last}></div>
                    </Col>
                </Row>
                <Row>                    
                    <NoteInput create={this.createNew}/>                    
                </Row>
            </Container>
        );
    }
}