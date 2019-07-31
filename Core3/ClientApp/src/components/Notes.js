import React, { Component } from 'react'
import Note from './Note'
import { getNotes } from './Fetch'

export default class Notes extends Component {
    constructor (props) {
        super(props);
        this.state = { notes: [], loading: true};
    }

    renderNotes = (notes) => {
        return (
            <div>
                {notes.map(n => (
                    <Note key={n.id} id={n.id} text={n.text}/>
                ))}
            </div>
        )
    }

    async componentDidMount() {
        const notes = await getNotes();
        this.setState({notes: notes, loading: false});
    }

    render(){
        const contents = this.state.loading
            ? <p><em>Loading...</em></p>
            : this.renderNotes(this.state.notes)

        return (
            <div className="note-container">
                {contents}
            </div>
        );
    }
}